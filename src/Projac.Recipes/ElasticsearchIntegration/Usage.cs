using System;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using Projac;
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
            var client = new ElasticLowLevelClient(config);
            var portfolioId = Guid.NewGuid();
            await new Projector<ElasticLowLevelClient>(Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                ProjectAsync(client, new object[]
                {
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static AnonymousProjection<ElasticLowLevelClient> Projection = new AnonymousProjectionBuilder<ElasticLowLevelClient>().
            When<PortfolioAdded>((client, message) =>
                client.IndexAsync<object>(
                    "index",
                    "portfolio",
                    message.Id.ToString("N"),
                    new PostData<object>(JsonConvert.SerializeObject(new
                    {
                        name = message.Name
                    })))).
            When<PortfolioRemoved>((client, message) =>
                client.DeleteAsync<object>(
                    "index",
                    "portfolio",
                    message.Id.ToString("N"))).
            When<PortfolioRenamed>((client, message) =>
                client.UpdateAsync<object>(
                    "index",
                    "portfolio", 
                    message.Id.ToString("N"), 
                    new PostData<object>(JsonConvert.SerializeObject(
                        new
                        {
                            Script = "ctx._source.name=name;",
                            Params = new
                            {
                                name = message.Name
                            }
                        })))).
            Build();
    }
}
