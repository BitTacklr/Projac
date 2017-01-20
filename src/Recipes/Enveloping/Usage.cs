using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Caching;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;

namespace Recipes.Enveloping
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
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
                        new Envelope(
                            new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                            new Dictionary<string, object>
                            {
                                {"Position", 0L}
                            }).ToGenericEnvelope(),
                        new Envelope(
                            new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                            new Dictionary<string, object>
                            {
                                {"Position", 1L}
                            }).ToGenericEnvelope(),
                        new Envelope(
                            new PortfolioRemoved {Id = portfolioId},
                            new Dictionary<string, object>
                            {
                                {"Position", 2L}
                            }).ToGenericEnvelope()
                    });
            }
        }

        class Envelope<TMessage> //Used by handlers
        {
            private readonly Envelope _envelope;

            public Envelope(Envelope envelope)
            {
                if (envelope == null)
                    throw new ArgumentNullException(nameof(envelope));
                _envelope = envelope;
            }

            public TMessage Message => (TMessage) _envelope.Message;
            public long Position => (long)_envelope.Metadata["Position"];
        }

        class Envelope //Used by dispatchers
        {
            //Note we could precompute these factories for all known message types.
            private static readonly ConcurrentDictionary<Type, Func<Envelope, object>> Factories = 
                new ConcurrentDictionary<Type, Func<Envelope, object>>();

            public Envelope(object message, IReadOnlyDictionary<string, object> metadata)
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));
                if (metadata == null)
                    throw new ArgumentNullException(nameof(metadata));
                Message = message;
                Metadata = metadata;
            }

            public object Message { get; }
            public IReadOnlyDictionary<string, object> Metadata { get; }

            public object ToGenericEnvelope()
            {
                var factory = Factories
                    .GetOrAdd(Message.GetType(), typeOfMessage =>
                    {
                        var parameter = Expression
                            .Parameter(typeof(Envelope), "envelope");
                        return Expression
                            .Lambda<Func<Envelope, object>>(
                                Expression.New(
                                    typeof(Envelope<>)
                                        .MakeGenericType(typeOfMessage)
                                        .GetConstructors()
                                        .Single(),
                                    parameter),
                                parameter)
                            .Compile();
                    });
                return factory(this);
            }
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
