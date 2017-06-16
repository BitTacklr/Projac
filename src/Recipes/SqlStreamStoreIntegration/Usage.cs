using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using Paramol.Executors;
using Paramol.SqlClient;
using Projac;
using Recipes.DataDefinition;
using Recipes.Shared;
using SqlStreamStore;
using SqlStreamStore.Streams;

namespace Recipes.SqlStreamStoreIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        [Test]
        public async Task ShowWithStream()
        {
            //setup a projection schema (one of many ways)
            var projector = new SqlProjector(
                Resolve.WhenEqualToHandlerMessageType(new PortfolioProjection()),
                new TransactionalSqlCommandExecutor(
                    new ConnectionStringSettings(
                        "projac",
                        @"Data Source=(localdb)\ProjectsV12;Initial Catalog=ProjacUsage;Integrated Security=SSPI;",
                        "System.Data.SqlClient"),
                    IsolationLevel.ReadCommitted));
            projector.Project(new object[] { new DropSchema(), new CreateSchema() });

            //setup a memory eventstore
            var store = new InMemoryStreamStore();

            //setup a sample stream (using some sample events)
            var portfolioId = Guid.NewGuid();
            var events = new object[]
            {
                new PortfolioAdded {Id = portfolioId, Name = "My Portfolio"},
                new PortfolioRenamed {Id = portfolioId, Name = "Your Portfolio"},
                new PortfolioRemoved {Id = portfolioId}
            };
            var stream = string.Format("portfolio-{0}", portfolioId.ToString("N"));
            await store.AppendToStream(
                stream,
                ExpectedVersion.Any,
                events
                    .Select(@event => new NewStreamMessage(
                        Guid.NewGuid(),
                        @event.GetType().FullName,
                        JsonConvert.SerializeObject(@event)))
                    .ToArray());

            //project the sample stream (until end of stream)
            var result =
                await store.ReadStreamForwards(stream, StreamVersion.Start, 1, true);
            foreach (var rawMessage in result.Messages)
            {
                var @event = JsonConvert.DeserializeObject(
                    await rawMessage.GetJsonData(),
                    Type.GetType(rawMessage.Type, true));

                projector.Project(@event);
            }

            while (!result.IsEnd)
            {
                result =
                    await store.ReadStreamForwards(stream, result.NextStreamVersion, 1, true);
                foreach (var rawMessage in result.Messages)
                {
                    var @event = JsonConvert.DeserializeObject(
                        await rawMessage.GetJsonData(),
                        Type.GetType(rawMessage.Type, true));

                    projector.Project(@event);
                }
            }
        }

        [Test]
        public async Task ShowWithCatchupSubscription()
        {
            //setup a projection schema (one of many ways)
            var projector = new SqlProjector(
                Resolve.WhenEqualToHandlerMessageType(new PortfolioProjection()),
                new TransactionalSqlCommandExecutor(
                    new ConnectionStringSettings(
                        "projac",
                        @"Data Source=(localdb)\ProjectsV12;Initial Catalog=ProjacUsage;Integrated Security=SSPI;",
                        "System.Data.SqlClient"),
                    IsolationLevel.ReadCommitted));
            projector.Project(new object[] { new DropSchema(), new CreateSchema() });

            //setup a memory eventstore
            var store = new InMemoryStreamStore();

            //setup a sample stream (using some sample events)
            var portfolioId = Guid.NewGuid();
            var events = new object[]
            {
                new PortfolioAdded {Id = portfolioId, Name = "My Portfolio"},
                new PortfolioRenamed {Id = portfolioId, Name = "Your Portfolio"},
                new PortfolioRemoved {Id = portfolioId}
            };
            var stream = string.Format("portfolio-{0}", portfolioId.ToString("N"));
            await store.AppendToStream(
                stream,
                ExpectedVersion.Any,
                events
                    .Select(@event => new NewStreamMessage(
                        Guid.NewGuid(),
                        @event.GetType().FullName,
                        JsonConvert.SerializeObject(@event)))
                    .ToArray());

            //project the sample stream (until end of stream)
            var subscription = store.SubscribeToStream(stream, null, async (_, rawMessage) =>
            {
                var @event = JsonConvert.DeserializeObject(
                    await rawMessage.GetJsonData(),
                    Type.GetType(rawMessage.Type, true));

                projector.Project(@event);
            });
            //should complete within 5 seconds.
            await Task.Delay(TimeSpan.FromSeconds(5));
            subscription.Dispose();
        }

        public class PortfolioProjection : SqlProjection
        {
            private static readonly SqlClientSyntax Sql = new SqlClientSyntax();

            public PortfolioProjection()
            {
                When<PortfolioAdded>(@event =>
                    Sql.NonQueryStatement(
                        "INSERT INTO [Portfolio] ([Id], [Name], [PhotoCount]) VALUES (@P1, @P2, 0)",
                        new {P1 = Sql.UniqueIdentifier(@event.Id), P2 = Sql.NVarChar(@event.Name, 40)}
                        ));
                When<PortfolioRemoved>(@event =>
                    Sql.NonQueryStatement(
                        "DELETE FROM [Portfolio] WHERE [Id] = @P1",
                        new {P1 = Sql.UniqueIdentifier(@event.Id)}
                        ));
                When<PortfolioRenamed>(@event =>
                    Sql.NonQueryStatement(
                        "UPDATE [Portfolio] SET [Name] = @P2 WHERE [Id] = @P1",
                        new {P1 = Sql.UniqueIdentifier(@event.Id), P2 = Sql.NVarChar(@event.Name, 40)}
                        ));

                When<CreateSchema>(_ =>
                    Sql.NonQueryStatement(
                        @"IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Portfolio' AND XTYPE='U')
                        BEGIN
                            CREATE TABLE [Portfolio] (
                                [Id] UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Portfolio PRIMARY KEY, 
                                [Name] NVARCHAR(MAX) NOT NULL,
                                [PhotoCount] INT NOT NULL)
                        END"));
                When<DropSchema>(_ =>
                    Sql.NonQueryStatement(
                        @"IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Portfolio' AND XTYPE='U')
                        DROP TABLE [Portfolio]"));
                When<DeleteData>(_ =>
                    Sql.NonQueryStatement(
                        @"IF EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Portfolio' AND XTYPE='U')
                        DELETE FROM [Portfolio]"));
            }
        }
    }
}
