using System;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Projac.Tests.Framework
{
    internal class DatabaseOperations
    {
        private static readonly string[] SupportedSqlServerInstances =
        {
            "(local)\\SQLEXPRESS",
            "(localdb)\\Projects",
            "(localdb)\\ProjectsV12",
            "(local)"
        };

        private static readonly string MdfPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projac.mdf");
        private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projac_log.ldf");

        private static readonly string[] AllPaths =
        {
            MdfPath,
            LogPath
        };

        public SqlServerInstanceDiscoveryResult DiscoverSqlServerInstance()
        {
            var result = DiscoverSqlServerInstanceUsingBruteForce();
            return result == SqlServerInstanceDiscoveryResult.NotFound ?
                DiscoverSqlServerInstanceUsingSqlDataSourceEnumerator() : 
                result;
        }

        private static SqlServerInstanceDiscoveryResult DiscoverSqlServerInstanceUsingSqlDataSourceEnumerator()
        {
            var dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
            for (var rowIndex = 0; rowIndex < dataSources.Rows.Count; rowIndex++)
            {
                var row = dataSources.Rows[rowIndex];
                var serverName = (string) row["ServerName"];
                var instanceName = row.IsNull("InstanceName") ? null : (string)row["InstanceName"];
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = string.IsNullOrEmpty(instanceName)
                        ? serverName
                        : string.Format("{0}\\{1}", serverName, instanceName),
                    IntegratedSecurity = true,
                    InitialCatalog = "master"
                };
                if (TryConnectToSqlServerInstance(builder))
                {
                    {
                        return SqlServerInstanceDiscoveryResult.Found(builder.DataSource);
                    }
                }
            }
            return SqlServerInstanceDiscoveryResult.NotFound;
        }

        private static SqlServerInstanceDiscoveryResult DiscoverSqlServerInstanceUsingBruteForce()
        {
            var builder = new SqlConnectionStringBuilder
            {
                IntegratedSecurity = true,
                InitialCatalog = "master"
            };

            foreach (var instance in SupportedSqlServerInstances)
            {
                builder.DataSource = instance;
                if (TryConnectToSqlServerInstance(builder))
                {
                    return SqlServerInstanceDiscoveryResult.Found(builder.DataSource);
                }
            }
            return SqlServerInstanceDiscoveryResult.NotFound;
        }

        private static bool TryConnectToSqlServerInstance(SqlConnectionStringBuilder builder)
        {
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    connection.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void RecreateDatabase(SqlServerInstanceDiscoveryResult result)
        {
            if (result == null) throw new ArgumentNullException("result");
            
            if (result == SqlServerInstanceDiscoveryResult.NotFound) return;

            DetachLocalDatabase(result.DataSource);
            DeleteLocalDatabaseFiles();
            CreateLocalDatabaseFiles(result.DataSource);
        }

        private static void CreateLocalDatabaseFiles(string dataSource)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                IntegratedSecurity = true,
                InitialCatalog = "master"
            };

            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = String.Format("CREATE DATABASE Projac ON PRIMARY(NAME=Projac, FILENAME='{0}')", MdfPath);
                    command.ExecuteNonQuery();
                    command.CommandText = "EXEC sp_detach_db 'Projac', 'true'";
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private static void DeleteLocalDatabaseFiles()
        {
            foreach (var path in AllPaths.Where(File.Exists))
            {
                File.Delete(path);
            }
        }

        public void DetachDatabase(SqlServerInstanceDiscoveryResult result)
        {
            if (result == null) throw new ArgumentNullException("result");
            
            if (result == SqlServerInstanceDiscoveryResult.NotFound) return;

            DetachLocalDatabase(result.DataSource);
        }

        private static void DetachLocalDatabase(string dataSource)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = dataSource,
                IntegratedSecurity = true,
                InitialCatalog = "master"
            };

            SqlConnection.ClearAllPools();
            using (var connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText =
                        "SELECT COUNT(*) FROM [sysdatabases] WHERE '[' + [name] + ']' = 'Projac' OR [name] = 'Projac'";
                    if (1 == (int) command.ExecuteScalar())
                    {
                        command.CommandText = "ALTER DATABASE [Projac] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                        command.ExecuteNonQuery();
                        command.CommandText = "EXEC sp_detach_db 'Projac', 'true'";
                        command.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }
    }
}
