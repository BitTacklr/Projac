using System;
using System.Runtime.Caching;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;

namespace Recipes.MemoryCacheIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class ProjectionUsage
    {
        [Test]
        public async Task Show()
        {
            using (var cache = new MemoryCache(new Random().Next().ToString()))
            {
                var portfolioId = Guid.NewGuid();
                await new ConnectedProjector<MemoryCache>(
                        Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                        ProjectAsync(cache, new object[]
                        {
                            new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                            new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                            new PortfolioRemoved {Id = portfolioId }
                        });
            }
        }

        public static AnonymousConnectedProjection<MemoryCache> Projection =
            new AnonymousConnectedProjectionBuilder<MemoryCache>().
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
                        var model = (PortfolioModel) item.Value;
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
}
