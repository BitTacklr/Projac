using System;
using System.Configuration;
using System.Threading;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class AsyncSqlNonQueryCommandExecutorTests
    {
        [Test]
        public void IsAsynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlNonQueryCommandExecutor>(SutFactory());
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
        public void ExecuteAsyncCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(async () => await sut.ExecuteAsync(null));
        }

        [Test]
        public void ExecuteAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(async () => await sut.ExecuteAsync(null, CancellationToken.None));
        }

        private static AsyncSqlNonQueryCommandExecutor SutFactory()
        {
            return SutFactory(ConnectionStringSettingsFactory("System.Data.SqlClient"));
        }

        private static AsyncSqlNonQueryCommandExecutor SutFactory(ConnectionStringSettings settings)
        {
            return new AsyncSqlNonQueryCommandExecutor(settings);
        }

        private static ConnectionStringSettings ConnectionStringSettingsFactory(string providerName)
        {
            return new ConnectionStringSettings("name", "", providerName);
        }

    }
}