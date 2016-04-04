using System.Security.Cryptography;
using System.Text;
using Paramol.SqlClient;
using Projac;

namespace Recipes.DataDefinition
{
    public static class Usage
    {
        private static readonly byte[] Id = "Sample".HashId();

        public class SampleUsingProjection : SqlClientProjection
        {
            public SampleUsingProjection()
            {
                When<CreateSchema>(_ =>
                    Sql.NonQueryStatement(
                        "CREATE TABLE [Sample] ([Id] INT NOT NULL CONSTRAINT PK_Sample PRIMARY KEY, [Value] INT NOT NULL)"));

                When<DropSchema>(_ =>
                    Sql.NonQueryStatement(
                        "DROP TABLE [Sample]"));

                When<DeleteData>(_ =>
                    Sql.NonQueryStatement(
                        "DELETE FROM [Sample]"));

                When<SetCheckpoint>(_ =>
                    Sql.NonQueryStatement(
                        "UPDATE [CheckpointGate] SET Checkpoint = @Checkpoint WHERE [Id] = @Id",
                        new
                        {
                            Checkpoint = Sql.BigInt(_.Checkpoint),
                            Id = Sql.Binary(Id, 16)
                        }));
            }
        }

        public class SqlClientProjection : SqlProjection
        {
            private static readonly SqlClientSyntax Syntax = new SqlClientSyntax();

            protected SqlClientSyntax Sql
            {
                get { return Syntax; }
            }
        }

        public static class SampleUsingBuilder
        {
            private static readonly SqlClientSyntax Sql = new SqlClientSyntax();

            public static readonly AnonymousSqlProjection Instance = new AnonymousSqlProjectionBuilder().
                When<CreateSchema>(_ =>
                    Sql.NonQueryStatement(
                        "CREATE TABLE [Sample] ([Id] INT NOT NULL CONSTRAINT PK_Sample PRIMARY KEY, [Value] INT NOT NULL)")).
                When<DropSchema>(_ =>
                    Sql.NonQueryStatement(
                        "DROP TABLE [Sample]")).
                When<DeleteData>(_ =>
                    Sql.NonQueryStatement(
                        "DELETE FROM [Sample]")).
                When<SetCheckpoint>(_ =>
                    Sql.NonQueryStatement(
                        "UPDATE [CheckpointGate] SET Checkpoint = @Checkpoint WHERE [Id] = @Id",
                        new
                        {
                            Checkpoint = Sql.BigInt(_.Checkpoint),
                            Id = Sql.Binary(Id, 16)
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

