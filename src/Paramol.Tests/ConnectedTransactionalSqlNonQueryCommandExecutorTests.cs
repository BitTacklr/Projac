using System;
using System.Data.Common;
using NUnit.Framework;
using Paramol.Tests.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class ConnectedTransactionalSqlNonQueryCommandExecutorTests
    {
        [Test]
        public void IsSynchronousTransactionalSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void TransactionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null));
        }

        [Test]
        public void ExecuteCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null));
        }

        private static ConnectedTransactionalSqlNonQueryCommandExecutor SutFactory()
        {
            return SutFactory(new TestDbTransaction(new TestDbConnection()));
        }

        private static ConnectedTransactionalSqlNonQueryCommandExecutor SutFactory(DbTransaction transaction)
        {
            return new ConnectedTransactionalSqlNonQueryCommandExecutor(transaction);
        }
    }
}