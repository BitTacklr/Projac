﻿using System;
using System.Collections.Generic;
#if NET46 || NET452
using System.Configuration;
#endif
using System.Threading;
using NUnit.Framework;
using Projac.Sql.Executors;

namespace Projac.Sql.Tests.Executors
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

#if NET46 || NET452
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
#endif

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
            Assert.That(async () =>
                await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>)null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteNonQueryAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteNonQueryAsync((IEnumerable<SqlNonQueryCommand>)null, CancellationToken.None),
                Throws.ArgumentNullException);
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
            Assert.That(async () =>
                await sut.ExecuteReaderAsync(null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteReaderAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteReaderAsync(null, CancellationToken.None),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteScalarCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ExecuteScalar(null));
        }

        [Test]
        public void ExecuteScalarAsyncCommandCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteScalarAsync(null),
                Throws.ArgumentNullException);
        }

        [Test]
        public void ExecuteScalarAsyncTokenCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.That(async () =>
                await sut.ExecuteScalarAsync(null, CancellationToken.None),
                Throws.ArgumentNullException);
        }

#if NETCOREAPP2_0
        private static SqlCommandExecutor SutFactory()
        {
            return new SqlCommandExecutor(System.Data.SqlClient.SqlClientFactory.Instance, "");
        }
#elif NET46 || NET452
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
#endif
    }
}
