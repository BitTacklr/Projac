using Paramol;
using Paramol.SqlClient;
using Projac;
using Usage.Messages;
using Usage.SystemMessages;

namespace Usage
{
    public static class PortfolioProjection
    {
        public static readonly SqlProjectionDescriptor Descriptor =
            new SqlProjectionDescriptorBuilder("photo:Portfolio", "v2")
            {
                SchemaProjection = new SqlProjectionBuilder().
                    When<BuildProjection>(_ => CreateTable()).
                    When<RebuildProjection>(_ => TSql.Compose(DropTableIfExists(), CreateTable())).
                    Build(),

                Projection = new SqlProjectionBuilder().
                    When<PortfolioAdded>(@event =>
                        TSql.NonQueryStatement(
                            "INSERT INTO [Portfolio] ([Id], [Name], [PhotoCount]) VALUES (@P1, @P2, 0)",
                            new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                            )).
                    When<PortfolioRemoved>(@event =>
                        TSql.NonQueryStatement(
                            "DELETE FROM [Portfolio] WHERE [Id] = @P1",
                            new {P1 = TSql.Int(@event.Id)}
                            )).
                    When<PortfolioRenamed>(@event =>
                        TSql.NonQueryStatement(
                            "UPDATE [Portfolio] SET [Name] = @P2 WHERE [Id] = @P1",
                            new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                            )).
                    Build()
            }.Build();

        private static SqlNonQueryCommand DropTableIfExists()
        {
            return TSql.NonQueryStatement(
@"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Portfolio'))
    BEGIN
	    EXEC('DROP TABLE [dbo].[Portfolio]')
    END"
                );
        }

        private static SqlNonQueryCommand CreateTable()
        {
            return TSql.NonQueryStatement(
@"CREATE TABLE [Portfolio] ( 
    [Id] INT NOT NULL CONSTRAINT [PK_Portfolio] PRIMARY KEY,
    [Name] NVARCHAR(MAX) NOT NULL,
    [PhotoCount] INT NOT NULL
)");
        }
    }
}