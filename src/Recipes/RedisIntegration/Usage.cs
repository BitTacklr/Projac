using System;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;
using StackExchange.Redis;

namespace Recipes.RedisIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        [Test]
        public async Task Show()
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions
            {
                EndPoints =
                {
                    {IPAddress.Parse("192.168.99.100"), 32770}
                }
            });
            var portfolioId = Guid.NewGuid();
            await new ConnectedProjector<ConnectionMultiplexer>(Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                ProjectAsync(connection, new object[]
                {
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static AnonymousConnectedProjection<ConnectionMultiplexer> Projection = new AnonymousConnectedProjectionBuilder<ConnectionMultiplexer>().
            When<PortfolioAdded>((connection, message) =>
            {
                var db = connection.GetDatabase();
                return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
            }).
            When<PortfolioRemoved>((connection, message) =>
            {
                var db = connection.GetDatabase();
                return db.HashDeleteAsync(message.Id.ToString("N"), "Name");
            }).
            When<PortfolioRenamed>((connection, message) =>
            {
                var db = connection.GetDatabase();
                return db.HashSetAsync(message.Id.ToString("N"), "Name", message.Name);
            }).
            Build();
    }
}
