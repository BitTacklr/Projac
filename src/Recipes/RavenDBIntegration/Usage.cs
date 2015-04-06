using System;
using System.IO;
using NUnit.Framework;
using Projac.RavenDB;
using Raven.Client.Embedded;
using Recipes.Shared;

namespace Recipes.RavenDBIntegration
{
    [TestFixture, Explicit]
    public class Usage
    {
        [Test]
        public async void Show()
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
                    await new AsyncRavenProjector(Projection.Handlers).
                        ProjectAsync(session, new object[]
                        {
                            new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                            new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                            new PortfolioRemoved {Id = portfolioId}
                        });
                }
            }
        }

        public static RavenProjection Projection = new RavenProjectionBuilder().
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
