using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Newtonsoft.Json;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;

namespace Recipes.ElasticsearchIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        [Test]
        public async Task Show()
        {
            //Spin up a docker image of elastic search and/or change the endpoint below
            var config = new ConnectionConfiguration(new Uri("http://192.168.99.100:32769/"));
            var client = new ElasticsearchClient(config);
            var portfolioId = Guid.NewGuid();
            await new ConnectedProjector<ElasticsearchClient>(Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                ProjectAsync(client, new object[]
                {
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static AnonymousConnectedProjection<ElasticsearchClient> Projection = new AnonymousConnectedProjectionBuilder<ElasticsearchClient>().
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
