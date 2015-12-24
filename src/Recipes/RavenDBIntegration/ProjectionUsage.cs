using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Raven.Client;
using Raven.Client.Embedded;
using Recipes.Shared;

namespace Recipes.RavenDBIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class ProjectionUsage
    {
        [Test]
        public async Task Show()
        {
            using (var store = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                DataDirectory = Path.GetTempPath()
            })
            {
                store.Initialize();
                using (var session = store.OpenAsyncSession())
                {
                    var portfolioId = Guid.NewGuid();
                    await new ConnectedProjector<IAsyncDocumentSession>(Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                        ProjectAsync(session, new object[]
                        {
                            new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                            new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                            new PortfolioRemoved {Id = portfolioId}
                        });
                }
            }
        }

        public static AnonymousConnectedProjection<IAsyncDocumentSession> Projection = new AnonymousConnectedProjectionBuilder<IAsyncDocumentSession>().
            When<PortfolioAdded>((session, message) => session.StoreAsync(
                new PortfolioDocument
                {
                    Id = message.Id,
                    Name = message.Name
                }, message.Id.ToString("N"))).
            When<PortfolioRemoved>(async (session, message) =>
            {
                var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
                session.Delete(document);
            }).
            When<PortfolioRenamed>(async (session, message) =>
            {
                var document = await session.LoadAsync<PortfolioDocument>(message.Id.ToString("N"));
                document.Name = message.Name;
            }).
            Build();

        class PortfolioDocument
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}
