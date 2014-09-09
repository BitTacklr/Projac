using System;
using System.Configuration;
using System.Data;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class TransactionalSqlNonQueryCommandExecutorTests
    {
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
        public void ExecuteCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null));
        }

        private static TransactionalSqlNonQueryCommandExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static TransactionalSqlNonQueryCommandExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new TransactionalSqlNonQueryCommandExecutor(settings, IsolationLevel.Unspecified);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }
    }
}