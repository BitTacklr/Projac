using System;
using System.Data.Common;
using NUnit.Framework;
using Paramol.Tests.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class ConnectedSqlNonQueryCommandExecutorTests
    {
        [Test]
        public void IsSynchronousSqlNonQueryCommandExecutor()
        {
            Assert.IsInstanceOf<ISqlNonQueryCommandExecutor>(SutFactory());
        }

        [Test]
        public void ConnectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory(null));
        }

        [Test]
        public void ConnectionMustBeOpen()
        {
            Assert.Throws<ArgumentException>(
                () => SutFactory(new TestDbConnection()));
        }

        [Test]
        public void OpenConnectionIsAccepted()
        {
            var connection = new TestDbConnection();
            connection.Open();
            Assert.DoesNotThrow(() => SutFactory(connection));
        }

        [Test]
        public void ExecuteCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Execute(null));
        }

        private static ConnectedSqlNonQueryCommandExecutor SutFactory()
        {
            var connection = new TestDbConnection();
            connection.Open();
            return SutFactory(connection);
        }

        private static ConnectedSqlNonQueryCommandExecutor SutFactory(DbConnection connection)
        {
            return new ConnectedSqlNonQueryCommandExecutor(connection);
        }
    }
}