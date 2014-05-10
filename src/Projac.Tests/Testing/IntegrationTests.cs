using System;
using System.Data.SqlClient;
using System.IO;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests.Testing
{
    [TestFixture, RequiresSqlServer]
    public class IntegrationTests
    {
        //[Test]
        //public void SetUp()
        //{
        //    var connectionBuilder = new SqlConnectionStringBuilder
        //    {
        //        DataSource = "(localdb)\\ProjectsV12",
        //        IntegratedSecurity = true,
        //        InitialCatalog = "master"
        //    };

        //    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projac.mdf");
        //    if(File.Exists(path))File.Delete(path);
        //    var path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Projac_log.ldf");
        //    if (File.Exists(path2)) File.Delete(path2);

        //    using (var connection = new SqlConnection(connectionBuilder.ConnectionString))
        //    {
        //        connection.Open();
        //        using (var command = new SqlCommand())
        //        {
        //            command.Connection = connection;
        //            command.CommandText = string.Format("CREATE DATABASE Projac ON PRIMARY(NAME=Projac, FILENAME='{0}')", path);
        //            command.ExecuteNonQuery();
        //            command.CommandText = "EXEC sp_detach_db 'Projac', 'true'";
        //            command.ExecuteNonQuery();
        //        }
        //        connection.Close();
        //    }
        //    connectionBuilder.InitialCatalog = "Projac";
        //    connectionBuilder.AttachDBFilename = "|DataDirectory|\\Projac.mdf";
        //    using (var connection = new SqlConnection(connectionBuilder.ConnectionString))
        //    {
        //        connection.Open();
        //        connection.Close();
        //    }
        //}
    }
}