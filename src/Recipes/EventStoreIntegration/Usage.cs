using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Embedded;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using NUnit.Framework;
using Paramol.Executors;
using Paramol.SqlClient;
using Projac;
using Recipes.DataDefinition;
using Recipes.Shared;

namespace Recipes.EventStoreIntegration
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

            //setup an embedded eventstore
            var node = EmbeddedVNodeBuilder.
                AsSingleNode().
                NoGossipOnPublicInterface().
                NoStatsOnPublicInterface().
                NoAdminOnPublicInterface().
                OnDefaultEndpoints().
                RunInMemory().
                Build();
            node.Start();

            var connection = EmbeddedEventStoreConnection.Create(node);
            await connection.ConnectAsync();

            //setup a sample stream (using some sample events)
            var portfolioId = Guid.NewGuid();
            var events = new object[]
            {
                new PortfolioAdded {Id = portfolioId, Name = "My Portfolio"},
                new PortfolioRenamed {Id = portfolioId, Name = "Your Portfolio"},
                new PortfolioRemoved {Id = portfolioId}
            };
            var stream = string.Format("portfolio-{0}", portfolioId.ToString("N"));
            var credentials = new UserCredentials("admin", "changeit");
            await connection.AppendToStreamAsync(
                stream,
                ExpectedVersion.Any,
                events.Select(@event => new EventData(
                    Guid.NewGuid(),
                    @event.GetType().FullName,
                    true,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                    new byte[0])).ToArray(),
                credentials);

            //project the sample stream (until end of stream)
            var result =
                await connection.ReadStreamEventsForwardAsync(stream, StreamPosition.Start, 1, false, credentials);
            while (!result.IsEndOfStream)
            {
                projector.Project(result.
                    Events.
                    Select(@event =>
                        JsonConvert.DeserializeObject(
                            Encoding.UTF8.GetString(@event.Event.Data),
                            Type.GetType(@event.Event.EventType, true))));
                result =
                    await connection.ReadStreamEventsForwardAsync(stream, result.NextEventNumber, 1, false, credentials);
            }
            projector.Project(result.
                    Events.
                    Select(@event =>
                        JsonConvert.DeserializeObject(
                            Encoding.UTF8.GetString(@event.Event.Data),
                            Type.GetType(@event.Event.EventType, true))));

            node.Stop();
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

            //setup an embedded eventstore
            var node = EmbeddedVNodeBuilder.
                AsSingleNode().
                NoGossipOnPublicInterface().
                NoStatsOnPublicInterface().
                NoAdminOnPublicInterface().
                OnDefaultEndpoints().
                RunInMemory().
                Build();
            node.Start();

            var connection = EmbeddedEventStoreConnection.Create(node);
            await connection.ConnectAsync();

            //setup a sample stream (using some sample events)
            var portfolioId = Guid.NewGuid();
            var events = new object[]
            {
                new PortfolioAdded {Id = portfolioId, Name = "My Portfolio"},
                new PortfolioRenamed {Id = portfolioId, Name = "Your Portfolio"},
                new PortfolioRemoved {Id = portfolioId}
            };
            var stream = string.Format("portfolio-{0}", portfolioId.ToString("N"));
            var credentials = new UserCredentials("admin", "changeit");
            await connection.AppendToStreamAsync(
                stream,
                ExpectedVersion.Any,
                events.Select(@event => new EventData(
                    Guid.NewGuid(),
                    @event.GetType().FullName,
                    true,
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                    new byte[0])).ToArray(),
                credentials);

            //project the sample stream (until end of stream)
            var subscription = connection.SubscribeToStreamFrom(stream, StreamPosition.Start, false, (_, @event) =>
            {
                projector.Project(
                    JsonConvert.DeserializeObject(
                        Encoding.UTF8.GetString(@event.Event.Data),
                        Type.GetType(@event.Event.EventType, true)));
            }, userCredentials: credentials);
            //should complete within 5 seconds.
            await Task.Delay(TimeSpan.FromSeconds(5));
            subscription.Stop();
            
            node.Stop();
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
