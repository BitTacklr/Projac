using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Projac.Connector.Testing;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Json.Linq;
using Recipes.Shared;

namespace Recipes.RavenDBIntegration
{
    [TestFixture, Explicit, Ignore("Must be run explicitly")]
    public class TestingUsage
    {
        [Test]
        public Task Show()
        {
            var portfolioId = Guid.NewGuid();
            return RavenProjectionScenario.For(Projection).
                Given(
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                ).
                ExpectNone();
        }

        public static AnonymousConnectedProjection<IAsyncDocumentSession> Projection = new AnonymousConnectedProjectionBuilder<IAsyncDocumentSession>().
            When<PortfolioAdded>((session, message) => session.StoreAsync(
                new PortfolioDocument
                {
                    Id = message.Id,
                    Name = message.Name
                })).
            When<PortfolioRemoved>(async (session, message) =>
            {
                var document = await session.LoadAsync<PortfolioDocument>(message.Id);
                session.Delete(document);
            }).
            When<PortfolioRenamed>(async (session, message) =>
            {
                var document = await session.LoadAsync<PortfolioDocument>(message.Id);
                document.Name = message.Name;
            }).
            Build();

        class PortfolioDocument
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }

    public static class RavenProjectionScenario
    {
        public static ConnectedProjectionScenario<IAsyncDocumentSession> For(
            ConnectedProjectionHandler<IAsyncDocumentSession>[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            return new ConnectedProjectionScenario<IAsyncDocumentSession>(
                ConcurrentResolve.WhenEqualToHandlerMessageType(handlers));
        }
    }

    public static class RavenTestingExtensions
    {
        public static Task ExpectNone(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario)
        {
            var specification = scenario.Verify(async session =>
            {
                var streamer = await session.Advanced.StreamAsync<RavenJObject>(Etag.Empty);
                if (await streamer.MoveNextAsync())
                {
                    var counter = 0;
                    do
                    {
                        counter++;
                    } while (await streamer.MoveNextAsync());
                    return VerificationResult.Fail(
                        string.Format("Expected no documents, but found {0} document(s).", counter));
                }
                return VerificationResult.Pass();
            });
            return specification.Assert();
        }

        public static Task Expect(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario, object[] documents)
        {
            var specification = scenario.Verify(session => Task.FromResult(VerificationResult.Pass("TODO")));
            return specification.Assert();
        }

        public static async Task Assert(this ConnectedProjectionTestSpecification<IAsyncDocumentSession> specification)
        {
            if (specification == null) throw new ArgumentNullException("specification");
            using (var store = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                DataDirectory = Path.GetTempPath()
            })
            {
                store.Configuration.Storage.Voron.AllowOn32Bits = true;
                store.Initialize();
                using (var session = store.OpenAsyncSession())
                {
                    await new ConnectedProjector<IAsyncDocumentSession>(specification.Resolver).
                        ProjectAsync(session, specification.Messages);
                    await session.SaveChangesAsync();

                    var result = await specification.Verification(session, CancellationToken.None);
                    if (result.Failed)
                    {
                        throw new AssertionException(result.Message);
                    }
                }
            }
        }
    }
}
