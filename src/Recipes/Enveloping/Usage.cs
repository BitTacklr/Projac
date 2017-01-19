using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;

namespace Recipes.Enveloping
{
    [TestFixture]//, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        [Test]
        public async Task Show()
        {
            using (var cache = new MemoryCache(new Random().Next().ToString()))
            {
                var portfolioId = Guid.NewGuid();
                await new ConnectedProjector<MemoryCache>(
                        Resolve.WhenEqualToHandlerMessageType(new Projection().Handlers)).
                    ProjectAsync(cache, new object[]
                    {
                        new Envelope<PortfolioAdded>(
                            new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                            new Dictionary<string, object>
                            {
                                {"Position", 0L}
                            }),
                        new Envelope<PortfolioRenamed>(
                            new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                            new Dictionary<string, object>
                            {
                                {"Position", 1L}
                            }),
                        new Envelope<PortfolioRemoved>(
                            new PortfolioRemoved {Id = portfolioId},
                            new Dictionary<string, object>
                            {
                                {"Position", 2L}
                            })
                    });
            }
        }

        class Envelope<TMessage>
        {
            public Envelope(TMessage message, IDictionary<string, object> metadata)
            {
                if (metadata == null)
                    throw new ArgumentNullException(nameof(metadata));
                Message = message;
                Metadata = metadata;
            }

            public TMessage Message { get; }
            public IDictionary<string, object> Metadata { get; }
            public long Position => (long)Metadata["Position"];
        }

        class Projection : ConnectedProjection<MemoryCache>
        {
            public Projection()
            {
                When<Envelope<PortfolioAdded>>((cache, envelope) =>
                {
                    cache.Add(
                        new CacheItem(
                            envelope.Message.Id.ToString(),
                            new PortfolioModel
                            {
                                Id = envelope.Message.Id,
                                Name = envelope.Message.Name,
                                Position = envelope.Position
                            }), new CacheItemPolicy
                            {
                                AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration
                            });
                });
                When<Envelope<PortfolioRemoved>>((cache, envelope) =>
                {
                    cache.Remove(envelope.Message.Id.ToString());
                });
                When<Envelope<PortfolioRenamed>>((cache, envelope) =>
                {
                    var item = cache.GetCacheItem(envelope.Message.Id.ToString());
                    if (item != null)
                    {
                        var model = (PortfolioModel)item.Value;
                        if (model.Position < envelope.Position)
                        {
                            model.Name = envelope.Message.Name;
                            model.Position = envelope.Position;
                        }
                    }
                });
            }
        }

        class PortfolioModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public long Position { get; set; }
        }
    }
}
