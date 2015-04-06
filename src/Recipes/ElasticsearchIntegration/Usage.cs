using System;
using Elasticsearch.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using Projac.Elasticsearch;
using Recipes.Shared;

namespace Recipes.ElasticsearchIntegration
{
    [TestFixture, Explicit]
    public class Usage
    {
        [Test]
        public async void Show()
        {
            var client = new ElasticsearchClient();
            var portfolioId = Guid.NewGuid();
            await new AsyncElasticsearchProjector(Projection.Handlers).
                ProjectAsync(client, new object[]
                {
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static ElasticsearchProjection Projection = new ElasticsearchProjectionBuilder().
            When<PortfolioAdded>((client, message) =>
                client.IndexAsync(
                    "index",
                    "portfolio",
                    message.Id.ToString("N"),
                    JsonConvert.SerializeObject(new
                    {
                        name = message.Name
                    }))).
            When<PortfolioRemoved>((client, message) =>
                client.DeleteAsync(
                    "index",
                    "portfolio",
                    message.Id.ToString("N"))).
            When<PortfolioRenamed>((client, message) =>
                client.UpdateAsync(
                    "index",
                    "portfolio", message.Id.ToString("N"), JsonConvert.SerializeObject(
                        new
                        {
                            Script = "ctx._source.name=name;",
                            Params = new
                            {
                                name = message.Name
                            }
                        }))).
            Build();
    }
}
