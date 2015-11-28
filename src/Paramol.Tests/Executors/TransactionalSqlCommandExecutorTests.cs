using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using NUnit.Framework;
using Paramol.Executors;

namespace Paramol.Tests.Executors
{
    [TestFixture]
    public class TransactionalSqlCommandExecutorTests
    {
        [Test]
        public void IsAsynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void IsSynchronousTransactionalSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryCommandExecutor>(SutFactory());
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
        public void ExecuteNonQueryCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ExecuteNonQuery((IEnumerable<SqlNonQueryCommand>) null));
        }

        [Test]
        public void ExecuteNonQueryAsyncCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () => 
                await sut.ExecuteNonQueryAsync((SqlNonQueryCommand)null), 
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteNonQueryAsyncTokenCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteNonQueryAsync((SqlNonQueryCommand)null, CancellationToken.None),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteNonQueryAsyncCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () => 
                await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>) null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteNonQueryAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>) null, CancellationToken.None),
                Throws.ArgumentNullException);
        }

        private static TransactionalSqlCommandExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static TransactionalSqlCommandExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new TransactionalSqlCommandExecutor(settings, IsolationLevel.Unspecified);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }
    }
}