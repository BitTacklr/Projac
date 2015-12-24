using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using Projac.Connector;
using Recipes.Shared;

namespace Recipes.WindowsAzureStorageIntegration
{
    [TestFixture, Ignore("Because 'Explicit' is not respected by R#")]
    public class Usage
    {
        [Test]
        public async Task ShowUsingDevStorage()
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var client = account.CreateCloudTableClient();
            var portfolioId = Guid.NewGuid();
            await new ConnectedProjector<CloudTableClient>(Resolve.WhenEqualToHandlerMessageType(Projection.Handlers)).
                ProjectAsync(client, new object[]
                {
                    new RebuildProjection(),
                    new PortfolioAdded {Id = portfolioId, Name = "My portfolio"},
                    new PortfolioRenamed {Id = portfolioId, Name = "Your portfolio"},
                    new PortfolioRemoved {Id = portfolioId}
                });
        }

        public static AnonymousConnectedProjection<CloudTableClient> Projection = new AnonymousConnectedProjectionBuilder<CloudTableClient>().
            When<RebuildProjection>((client, message) =>
            {
                var table = client.GetTableReference("Portfolio");
                return table.CreateIfNotExistsAsync();
            }).
            When<PortfolioAdded>((client, message) =>
            {
                var table = client.GetTableReference("Portfolio");
                return table.ExecuteAsync(
                    TableOperation.Insert(
                    new PortfolioModel(message.Id)
                    {
                        Name = message.Name
                    }));
            }).
            When<PortfolioRemoved>((client, message) =>
            {
                var table = client.GetTableReference("Portfolio");
                return table.ExecuteAsync(
                    TableOperation.Delete(new PortfolioModel(message.Id)
                    {
                        ETag = "*"
                    }));   
            }).
            When<PortfolioRenamed>((client, message) =>
            {
                var table = client.GetTableReference("Portfolio");
                return table.ExecuteAsync(
                    TableOperation.Merge(new PortfolioModel(message.Id)
                    {
                        Name = message.Name,
                        ETag = "*"
                    }));
            }).
            Build();
    }
}
