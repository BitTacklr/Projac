using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Recipes.AzureStorageIntegration
{
    public class PortfolioModel : TableEntity
    {
        public PortfolioModel(Guid id)
        {
            PartitionKey = "Portfolio";
            RowKey = id.ToString("N");
        }

        public string Name { get; set; }
    }
}