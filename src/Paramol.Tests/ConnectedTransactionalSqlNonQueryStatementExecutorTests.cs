using System;
using System.Data.Common;
using NUnit.Framework;
using Paramol.Tests.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class ConnectedTransactionalSqlNonQueryStatementExecutorTests
    {
        [Test]
        public void IsSynchronousTransactionalSqlNonQueryStatementExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryStatementExecutor>(SutFactory());
        }

        [Test]
        public void TransactionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null));
        }

        [Test]
        public void ExecuteStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null));
        }

        private static ConnectedTransactionalSqlNonQueryStatementExecutor SutFactory()
        {
            return SutFactory(new TestDbTransaction(new TestDbConnection()));
        }

        private static ConnectedTransactionalSqlNonQueryStatementExecutor SutFactory(DbTransaction transaction)
        {
            return new ConnectedTransactionalSqlNonQueryStatementExecutor(transaction);
        }
    }
}