using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class AsyncSqlProjectorTests
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
                () => SutFactory((IAsyncSqlNonQueryStatementExecutor)null));

        }

        [Test]
        public void ProjectAsyncEventCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(null));
        }

        [Test]
        public void ProjectAsyncTokenEventCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(null, CancellationToken.None));
        }

        [Test]
        public async void ProjectAsyncCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMatches()
        {
            var statements = new[] { StatementFactory(), StatementFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => statements);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object());

            Assert.That(mock.Statements, Is.EquivalentTo(statements));
        }

        [Test]
        public async void ProjectAsyncCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMismatches()
        {
            var statements = new[] { StatementFactory(), StatementFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => statements);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object());

            Assert.That(mock.Statements, Is.Empty);
        }

        [Test]
        public async void ProjectAsyncTokenCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMatches()
        {
            var statements = new[] { StatementFactory(), StatementFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => statements);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object(), CancellationToken.None);

            Assert.That(mock.Statements, Is.EquivalentTo(statements));
        }

        [Test]
        public async void ProjectAsyncTokenCausesExecutorToBeCalledWithExpectedStatementsWhenEventTypeMismatches()
        {
            var statements = new[] { StatementFactory(), StatementFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => statements);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object(), CancellationToken.None);

            Assert.That(mock.Statements, Is.Empty);
        }

        private static AsyncSqlProjector SutFactory()
        {
            return SutFactory(new SqlProjectionHandler[0], new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandler[] handlers)
        {
            return SutFactory(handlers, new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(IAsyncSqlNonQueryStatementExecutor executor)
        {
            return SutFactory(new SqlProjectionHandler[0], executor);
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandler[] handlers, IAsyncSqlNonQueryStatementExecutor executor)
        {
            return new AsyncSqlProjector(handlers, executor);
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new DbParameter[0]);
        }

        class ExecutorMock : IAsyncSqlNonQueryStatementExecutor
        {
            public readonly List<SqlNonQueryStatement> Statements = new List<SqlNonQueryStatement>();

            public Task ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements)
            {
                return ExecuteAsync(statements, CancellationToken.None);
            }

            public Task ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements, CancellationToken cancellationToken)
            {
                Statements.AddRange(statements);
                return Task.FromResult<object>(null);
            }
        }

        class ExecutorStub : IAsyncSqlNonQueryStatementExecutor
        {
            public Task ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements)
            {
                return Task.FromResult<object>(null);
            }

            public Task ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements, CancellationToken cancellationToken)
            {
                return Task.FromResult<object>(null);
            }
        }
    }
}