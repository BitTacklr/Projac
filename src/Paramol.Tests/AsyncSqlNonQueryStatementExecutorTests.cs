using System;
using System.Configuration;
using System.Threading;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class AsyncSqlNonQueryStatementExecutorTests
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

        private static AsyncSqlNonQueryStatementExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static AsyncSqlNonQueryStatementExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new AsyncSqlNonQueryStatementExecutor(settings);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }

    }
}