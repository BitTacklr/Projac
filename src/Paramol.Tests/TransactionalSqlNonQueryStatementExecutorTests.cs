using System;
using System.Configuration;
using System.Data;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class TransactionalSqlNonQueryStatementExecutorTests
    {
        [Test]
        public void IsSynchronousTransactionalSqlNonQueryStatementExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryStatementExecutor>(SutFactory());
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
        public void ExecuteStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null));
        }

        private static TransactionalSqlNonQueryStatementExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static TransactionalSqlNonQueryStatementExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new TransactionalSqlNonQueryStatementExecutor(settings, IsolationLevel.Unspecified);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }
    }
}