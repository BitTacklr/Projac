using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class TestingUsage
    {
        [Test]
        public Task ShowExpectNone()
        {
            var portfolioId = Guid.NewGuid();
            return RavenProjectionScenario.For(Projection)
                .Given(
                    new PortfolioAdded { Id = portfolioId, Name = "My portfolio" },
                    new PortfolioRenamed { Id = portfolioId, Name = "Your portfolio" },
                    new PortfolioRemoved { Id = portfolioId }
                )
                .ExpectNone();
        }

        [Test]
        public Task ShowExpect()
        {
            var portfolioId = Guid.NewGuid();
            return RavenProjectionScenario.For(Projection)
                .Given(
                    new PortfolioAdded { Id = portfolioId, Name = "My portfolio" },
                    new PortfolioRenamed { Id = portfolioId, Name = "Your portfolio" }
                )
                .Expect(new PortfolioDocument
                {
                    Id = portfolioId,
                    Name = "Your portfolio"
                });
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

        public static Task ExpectNone(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario)
        {
            return scenario
                .Verify(async session =>
                {
                    using (var streamer = await session.Advanced.StreamAsync<RavenJObject>(Etag.Empty))
                    {
                        if (await streamer.MoveNextAsync())
                        {
                            var storedDocumentIdentifiers = new List<string>();
                            do
                            {
                                storedDocumentIdentifiers.Add(streamer.Current.Key);
                            } while (await streamer.MoveNextAsync());

                            return VerificationResult.Fail(
                                string.Format("Expected no documents, but found {0} document(s) ({1}).",
                                    storedDocumentIdentifiers.Count,
                                    string.Join(",", storedDocumentIdentifiers)));
                        }
                        return VerificationResult.Pass();
                    }
                })
                .Assert();
        }

        public static Task Expect(this ConnectedProjectionScenario<IAsyncDocumentSession> scenario, params object[] documents)
        {
            if (documents == null) 
                throw new ArgumentNullException("documents");

            if (documents.Length == 0)
            {
                return scenario.ExpectNone();
            }
            return scenario
                .Verify(async session =>
                {
                    using (var streamer = await session.Advanced.StreamAsync<object>(Etag.Empty))
                    {
                        var storedDocuments = new List<object>();
                        var storedDocumentIdentifiers = new List<string>();
                        while (await streamer.MoveNextAsync())
                        {
                            storedDocumentIdentifiers.Add(streamer.Current.Key);
                            storedDocuments.Add(streamer.Current.Document);
                        }

                        if (documents.Length != storedDocumentIdentifiers.Count)
                        {
                            if (storedDocumentIdentifiers.Count == 0)
                            {
                                return VerificationResult.Fail(
                                    string.Format("Expected {0} document(s), but found 0 documents.",
                                        documents.Length));
                            }
                            return VerificationResult.Fail(
                                string.Format("Expected {0} document(s), but found {1} document(s) ({2}).",
                                    documents.Length,
                                    storedDocumentIdentifiers.Count,
                                    string.Join(",", storedDocumentIdentifiers)));
                        }

                        var expectedDocuments = documents.Select(JToken.FromObject).ToArray();
                        var actualDocuments = storedDocuments.Select(JToken.FromObject).ToArray();

                        if (!expectedDocuments.SequenceEqual(actualDocuments, new JTokenEqualityComparer()))
                        {
                            var builder = new StringBuilder();
                            builder.AppendLine("Expected the following documents:");
                            foreach (var expectedDocument in expectedDocuments)
                            {
                                builder.AppendLine(expectedDocument.ToString());
                            }
                            builder.AppendLine();
                            builder.AppendLine("But found the following documents:");
                            foreach (var actualDocument in actualDocuments)
                            {
                                builder.AppendLine(actualDocument.ToString());
                            }
                            return VerificationResult.Fail(builder.ToString());
                        }
                        return VerificationResult.Pass();
                    }
                })
                .Assert();
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
