using System;
using System.Configuration;
using System.Data;
using System.Threading;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class TransactionalAsyncSqlNonQueryStatementExecutorTests
    {
        [Test]
        public void IsAsynchronousSqlNonQueryStatementExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlNonQueryStatementExecutor>(SutFactory());
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
        public void ExecuteAsyncStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(async () => await sut.ExecuteAsync(null));
        }

        [Test]
        public void ExecuteAsyncTokenStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(async () => await sut.ExecuteAsync(null, CancellationToken.None));
        }

        private static TransactionalAsyncSqlNonQueryStatementExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static TransactionalAsyncSqlNonQueryStatementExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new TransactionalAsyncSqlNonQueryStatementExecutor(settings, IsolationLevel.Unspecified);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }

    }
}