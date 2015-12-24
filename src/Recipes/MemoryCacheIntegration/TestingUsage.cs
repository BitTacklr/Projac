using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KellermanSoftware.CompareNetObjects;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Projac.Connector;
using Projac.Connector.Testing;
using Recipes.Shared;

namespace Recipes.MemoryCacheIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class TestingUsage
    {
        [Test]
        public Task ShowExpectNone()
        {
            var portfolioId = Guid.NewGuid();
            return MemoryCacheProjection.For(Projection)
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
            return MemoryCacheProjection.For(Projection)
                .Given(
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"}
                )
                .Expect(
                    new CacheItem(
                        portfolioId.ToString(),
                        new PortfolioModel
                        {
                            Id = portfolioId,
                            Name = "Your portfolio"
                        }));
        }

        public static AnonymousConnectedProjection<MemoryCache> Projection = new AnonymousConnectedProjectionBuilder<MemoryCache>().
            When<PortfolioAdded>((cache, message) =>
            {
                cache.Add(
                    new CacheItem(
                        message.Id.ToString(),
                        new PortfolioModel
                        {
                            Id = message.Id,
                            Name = message.Name
                        }), new CacheItemPolicy
                        {
                            AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
                        });
                }).
            When<PortfolioRemoved>((cache, message) =>
            {
                cache.Remove(message.Id.ToString());
            }).
            When<PortfolioRenamed>((cache, message) =>
            {
                var item = cache.GetCacheItem(message.Id.ToString());
                if (item != null)
                {
                    var model = (PortfolioModel)item.Value;
                    model.Name = message.Name;
                }
            }).
            Build();

        class PortfolioModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }

    public static class MemoryCacheProjection
    {
        public static ConnectedProjectionScenario<MemoryCache> For(ConnectedProjectionHandler<MemoryCache>[] handlers)
        {
            return new ConnectedProjectionScenario<MemoryCache>(
                ConcurrentResolve.WhenEqualToHandlerMessageType(handlers)
                );
        }

        public static Task ExpectNone(this ConnectedProjectionScenario<MemoryCache> scenario)
        {
            return scenario.
                Verify(cache =>
                {
                    if (cache.GetCount() != 0)
                    {
                        return Task.FromResult(
                            VerificationResult.Fail(
                                string.Format("Expected no cache items, but found {0} cache item(s) ({1}).",
                                    cache.GetCount(),
                                    string.Join(",", cache.Select(pair => pair.Key)))));
                    }
                    return Task.FromResult(VerificationResult.Pass());
                }).
                Assert();
        }

        public static async Task Assert(this ConnectedProjectionTestSpecification<MemoryCache> specification)
        {
            if (specification == null)
                throw new ArgumentNullException("specification");
            using (var cache = new MemoryCache(new Random().Next().ToString()))
            {
                await new ConnectedProjector<MemoryCache>(specification.Resolver).
                    ProjectAsync(cache, specification.Messages);
                var result = await specification.Verification(cache, CancellationToken.None);
                if (result.Failed)
                {
                    throw new AssertionException(result.Message);
                }
            }
        }

        public static Task Expect(this ConnectedProjectionScenario<MemoryCache> scenario, params CacheItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (items.Length == 0)
            {
                return scenario.ExpectNone();
            }

            return scenario.
                Verify(cache =>
                {
                    if (cache.GetCount() != items.Length)
                    {
                        if (cache.GetCount() == 0)
                        {
                            return Task.FromResult(
                                VerificationResult.Fail(
                                    string.Format("Expected {0} cache item(s), but found 0 cache items.",
                                        items.Length)));
                        }

                        return Task.FromResult(
                            VerificationResult.Fail(
                                string.Format("Expected {0} cache item(s), but found {1} cache item(s) ({2}).",
                                    items.Length,
                                    cache.GetCount(),
                                    string.Join(",", cache.Select(pair => pair.Key)))));
                    }
                    if (!cache.Select(pair => cache.GetCacheItem(pair.Key)).SequenceEqual(items, new CacheItemEqualityComparer()))
                    {
                        var builder = new StringBuilder();
                        builder.AppendLine("Expected the following cache items:");
                        foreach (var expectedItem in items)
                        {
                            builder.AppendLine(expectedItem.Key + ": " + JToken.FromObject(expectedItem.Value).ToString());
                        }
                        builder.AppendLine();
                        builder.AppendLine("But found the following cache items:");
                        foreach (var actualItem in cache)
                        {
                            builder.AppendLine(actualItem.Key + ": " + JToken.FromObject(actualItem.Value).ToString());
                        }
                        return Task.FromResult(VerificationResult.Fail(builder.ToString()));
                    }
                    return Task.FromResult(VerificationResult.Pass());
                }).
                Assert();
        }
    }

    public class CacheItemEqualityComparer : IEqualityComparer<CacheItem>
    {
        public bool Equals(CacheItem x, CacheItem y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;
            return x.Key.Equals(y.Key) &&
                   new CompareLogic().Compare(x.Value, y.Value).AreEqual;
        }

        public int GetHashCode(CacheItem obj)
        {
            throw new NotSupportedException();
        }
    }
}
