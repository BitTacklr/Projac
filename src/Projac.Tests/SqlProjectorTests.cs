using System;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectorTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((SqlProjectionHandler[])null));
        }

        [Test]
        public void ExecutorCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((ISqlNonQueryStatementExecutor)null));
            
        }

        [Test]
        public void ProjectEventCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Project(null));
        }

        [Test]
        public void ProjectCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMatches()
        {
            var statements = new[] {StatementFactory(), StatementFactory()};
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => statements);
            var sut = SutFactory(new[] {handler}, mock);

            var result = sut.Project(new object());

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Statements, Is.EquivalentTo(statements));
        }

        [Test]
        public void ProjectCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMismatches()
        {
            var statements = new[] { StatementFactory(), StatementFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => statements);
            var sut = SutFactory(new[] { handler }, mock);

            var result = sut.Project(new object());

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Statements, Is.Empty);
        }

        private static SqlProjector SutFactory()
        {
            return SutFactory(new SqlProjectionHandler[0], new ExecutorStub());
        }

        private static SqlProjector SutFactory(SqlProjectionHandler[] handlers)
        {
            return SutFactory(handlers, new ExecutorStub());
        }

        private static SqlProjector SutFactory(ISqlNonQueryStatementExecutor executor)
        {
            return SutFactory(new SqlProjectionHandler[0], executor);
        }

        private static SqlProjector SutFactory(SqlProjectionHandler[] handlers, ISqlNonQueryStatementExecutor executor)
        {
            return new SqlProjector(handlers, executor);
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new DbParameter[0]);
        }

        class ExecutorMock : ISqlNonQueryStatementExecutor
        {
            public readonly List<SqlNonQueryStatement> Statements = new List<SqlNonQueryStatement>();

            public int Execute(IEnumerable<SqlNonQueryStatement> statements)
            {
                var count = Statements.Count;
                Statements.AddRange(statements);
                return Statements.Count - count;
            }
        }

        class ExecutorStub : ISqlNonQueryStatementExecutor
        {
            public int Execute(IEnumerable<SqlNonQueryStatement> statements)
            {
                return 0;
            }
        }
    }
}
