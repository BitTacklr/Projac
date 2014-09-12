using System.Configuration;
using Paramol;
using Paramol.SqlClient;
using Projac;

namespace Usage
{
    class Program
    {
        static void Main()
        {
            var settings = new ConnectionStringSettings("Target",
                "Data Source=(localdb)\\ProjectsV12;Initial Catalog=CashCow;Integrated Security=SSPI",
                "System.Data.SqlClient");
            var executor = new SqlNonQueryCommandExecutor(settings);
            var schemaProjector = new SqlProjector(PortfolioProjection.Descriptor.SchemaProjection.Handlers, executor);
            var projector = new SqlProjector(PortfolioProjection.Descriptor.Projection.Handlers, executor);
            //Build
            schemaProjector.Project(new BuildProjection(PortfolioProjection.Descriptor.Identifier));
            projector.Project(new PortfolioAdded {Id = 123, Name = "Portfolio"});
            //Rebuild
            schemaProjector.Project(new RebuildProjection(PortfolioProjection.Descriptor.Identifier));
            projector.Project(new PortfolioAdded { Id = 123, Name = "Portfolio" });
        }
    }

    public static class PortfolioProjection
    {
        public static readonly SqlProjectionDescriptor Descriptor =
            new SqlProjectionDescriptorBuilder("photo:Portfolio:v1")
            {
                SchemaProjection = new SqlProjectionBuilder().
                    When<BuildProjection>(_ =>
                        TSql.NonQueryStatement(
                            @"CREATE TABLE [Portfolio] ( 
                                [Id] INT NOT NULL CONSTRAINT [PK_Portfolio] PRIMARY KEY,
                                [Name] NVARCHAR(MAX) NOT NULL,
                                [PhotoCount] INT NOT NULL
                            )")).
                    When<RebuildProjection>(_ =>
                        TSql.Compose(
                        TSql.NonQueryStatement(
                            @"IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Portfolio'))
	                            BEGIN
		                            EXEC('DROP TABLE [dbo].[Portfolio]')
	                            END"
                        ),
                        TSql.NonQueryStatement(
                            @"CREATE TABLE [Portfolio] ( 
                                [Id] INT NOT NULL CONSTRAINT [PK_Portfolio] PRIMARY KEY,
                                [Name] NVARCHAR(MAX) NOT NULL,
                                [PhotoCount] INT NOT NULL
                            )"))
                    ).Build(),

                Projection = new SqlProjectionBuilder().
                    When<PortfolioAdded>(@event =>
                        TSql.NonQueryStatement(
                            "INSERT INTO [Portfolio] (Id, Name, PhotoCount) VALUES (@P1, @P2, 0)",
                            new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                            )).
                    When<PortfolioRemoved>(@event =>
                        TSql.NonQueryStatement(
                            "DELETE FROM [Portfolio] WHERE Id = @P1",
                            new {P1 = TSql.Int(@event.Id)}
                            )).
                    When<PortfolioRenamed>(@event =>
                        TSql.NonQueryStatement(
                            "UPDATE [Portfolio] SET Name = @P2 WHERE Id = @P1",
                            new {P1 = TSql.Int(@event.Id), P2 = TSql.NVarChar(@event.Name, 40)}
                            )).
                    Build()
            }.Build();
    }

    public class PortfolioRenamed
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PortfolioRemoved
    {
        public int Id { get; set; }
    }

    public class PortfolioAdded
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RebuildProjection
    {
        public readonly string Identifier;

        public RebuildProjection(string identifier)
        {
            Identifier = identifier;
        }
    }

    public class BuildProjection
    {
        public readonly string Identifier;

        public BuildProjection(string identifier)
        {
            Identifier = identifier;
        }
    }
}
