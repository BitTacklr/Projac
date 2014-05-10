using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace Projac.Tests.Framework
{
    internal static class TestDatabase
    {
        private const string CallContextKey = "Projac.SqlServerInstanceDiscoveryResult";

        private static SqlServerInstanceDiscoveryResult GetDiscoveryResult()
        {
            return (SqlServerInstanceDiscoveryResult)CallContext.GetData(CallContextKey);
        }

        public static SqlConnection OpenConnection()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = GetDiscoveryResult().DataSource,
                IntegratedSecurity = true,
                InitialCatalog = "Projac",
                AttachDBFilename = "|DataDirectory|\\Projac.mdf"
            };
            var connection = new SqlConnection(builder.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}