using System.Security.Cryptography;
using System.Text;
using Paramol.SqlClient;
using Projac;

namespace Recipes.DataDefinition
{
    public static class Usage
    {
        private static readonly byte[] Id = "Sample".HashId();

        public class SampleUsingProjection : SqlProjection
        {
            public SampleUsingProjection()
            {
                When<CreateSchema>(_ =>
                    TSql.NonQueryStatement(
                        "CREATE TABLE [Sample] ([Id] INT NOT NULL CONSTRAINT PK_Sample PRIMARY KEY, [Value] INT NOT NULL)"));

                When<DropSchema>(_ =>
                    TSql.NonQueryStatement(
                        "DROP TABLE [Sample]"));

                When<DeleteData>(_ =>
                    TSql.NonQueryStatement(
                        "DELETE FROM [Sample]"));

                When<SetCheckpoint>(_ =>
                    TSql.NonQueryStatement(
                        "UPDATE [CheckpointGate] SET Checkpoint = @Checkpoint WHERE [Id] = @Id",
                        new
                        {
                            Checkpoint = TSql.BigInt(_.Checkpoint),
                            Id = TSql.Binary(Id, 16)
                        }));
            }
        }

        public static class SampleUsingBuilder
        {
            public static readonly SqlProjectionHandler[] Handlers = new SqlProjectionBuilder().
                When<CreateSchema>(_ =>
                    TSql.NonQueryStatement(
                        "CREATE TABLE [Sample] ([Id] INT NOT NULL CONSTRAINT PK_Sample PRIMARY KEY, [Value] INT NOT NULL)")).
                When<DropSchema>(_ =>
                    TSql.NonQueryStatement(
                        "DROP TABLE [Sample]")).
                When<DeleteData>(_ =>
                    TSql.NonQueryStatement(
                        "DELETE FROM [Sample]")).
                When<SetCheckpoint>(_ =>
                    TSql.NonQueryStatement(
                        "UPDATE [CheckpointGate] SET Checkpoint = @Checkpoint WHERE [Id] = @Id",
                        new
                        {
                            Checkpoint = TSql.BigInt(_.Checkpoint),
                            Id = TSql.Binary(Id, 16)
                        })).
                Build();
        }

        private static byte[] HashId(this string value)
        {
            using (var hash = MD5.Create())
                return hash.ComputeHash(Encoding.UTF8.GetBytes(value));
        }
    }
}

