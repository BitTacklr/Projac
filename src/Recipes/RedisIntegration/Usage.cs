using System;
using System.Net;
using NUnit.Framework;
using Projac.Redis;
using Recipes.Shared;
using StackExchange.Redis;

namespace Recipes.RedisIntegration
{
    [TestFixture, Explicit]
    public class Usage
    {
        [Test]
        public async void Show()
        {
            var connection = await ConnectionMultiplexer.ConnectAsync(new ConfigurationOptions
            {
                EndPoints =
                {
                    {IPAddress.Loopback, 6379}
                }
            });
            var portfolioId = Guid.NewGuid();
            await new AsyncRedisProjector(Projection.Handlers).
                ProjectAsync(connection, new object[]
                {
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static RedisProjection Projection = new RedisProjectionBuilder().
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
