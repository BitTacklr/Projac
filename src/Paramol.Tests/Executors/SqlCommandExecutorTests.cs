using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using NUnit.Framework;
using Paramol.Executors;

namespace Paramol.Tests.Executors
{
    [TestFixture]
    public class SqlCommandExecutorTests
    {
        [Test]
        public void IsAsynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void IsSynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void IsSynchronousSqlQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void IsAsynchronousSqlQueryCommandExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void ConnectionStringSettingsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null));
        }

        [Test]
        public void SqlClientProviderNameIsSupported()
        {
            Assert.DoesNotThrow(
                () => SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient")));
        }

        [Test]
        public void NonExistingProviderNameThrows()
        {
            Assert.Throws<ArgumentException>(
                () => SutFactory(ConnectionStringSettingsFactory(Guid.NewGuid().ToString("N"))));
        }

        [Test]
        public void ExecuteNonQueryCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ExecuteNonQuery((SqlNonQueryCommand)null));
        }


        [Test]
        public void ExecuteNonQueryAsyncCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteNonQueryAsync((SqlNonQueryCommand)null));
        }

        [Test]
        public void ExecuteNonQueryAsyncTokenCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteNonQueryAsync((SqlNonQueryCommand)null, CancellationToken.None));
        }

        [Test]
        public void ExecuteNonQueryCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ExecuteNonQuery((IEnumerable<SqlNonQueryCommand>) null));
        }

        [Test]
        public void ExecuteNonQueryAsyncCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>) null));
        }

        [Test]
        public void ExecuteNonQueryAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>) null, CancellationToken.None));
        }

        [Test]
        public void ExecuteReaderCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ExecuteReader(null));
        }

        [Test]
        public void ExecuteReaderAsyncCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteReaderAsync(null));
        }

        [Test]
        public void ExecuteReaderAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                async () => await sut.ExecuteReaderAsync(null, CancellationToken.None));
        }

        private static SqlCommandExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static SqlCommandExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new SqlCommandExecutor(settings);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }
    }
}
