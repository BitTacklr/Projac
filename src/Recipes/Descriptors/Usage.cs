using Paramol.SqlClient;
using Projac;
using Recipes.Shared;

namespace Recipes.Descriptors
{
    public static class Usage
    {
        public static readonly SqlProjectionDescriptor Portfolio =
            new SqlProjectionDescriptorBuilder
            {
                Projection = new SqlProjectionBuilder().
                    When<PortfolioAdded>(@event =>
                        TSql.NonQueryStatement(
                            "INSERT INTO [Portfolio] ([Id], [Name], [PhotoCount]) VALUES (@P1, @P2, 0)",
                            new { P1 = TSql.UniqueIdentifier(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
                            )).
                    When<PortfolioRemoved>(@event =>
                        TSql.NonQueryStatement(
                            "DELETE FROM [Portfolio] WHERE [Id] = @P1",
                            new { P1 = TSql.UniqueIdentifier(@event.Id) }
                            )).
                    When<PortfolioRenamed>(@event =>
                        TSql.NonQueryStatement(
                            "UPDATE [Portfolio] SET [Name] = @P2 WHERE [Id] = @P1",
                            new { P1 = TSql.UniqueIdentifier(@event.Id), P2 = TSql.NVarChar(@event.Name, 40) }
                            )).
                    Build()
            }.Build();
    }
}
