using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using NUnit.Framework;
using Paramol.Executors;
using Paramol.Tests.Framework;

namespace Paramol.Tests.Executors
{
    [TestFixture]
    public class ConnectedTransactionalSqlCommandExecutorTests
    {
        [Test]
        public void IsSynchronousTransactionalSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void IsAsynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<IAsyncSqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void TransactionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null));
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

        private static ConnectedTransactionalSqlCommandExecutor SutFactory()
        {
            return SutFactory(new TestDbTransaction(new TestDbConnection()));
        }

        private static ConnectedTransactionalSqlCommandExecutor SutFactory(DbTransaction transaction)
        {
            return new ConnectedTransactionalSqlCommandExecutor(transaction);
        }
    }
}