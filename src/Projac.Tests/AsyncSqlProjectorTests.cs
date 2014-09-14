using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Paramol;
using Paramol.Executors;
using Projac.Tests.Framework;

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
                () => SutFactory((IAsyncSqlNonQueryCommandExecutor)null));

        }

        [Test]
        public void ProjectAsyncMessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(null));
        }

        [Test]
        public void ProjectAsyncTokenMessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(null, CancellationToken.None));
        }

        [Test]
        public async void ProjectAsyncCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object());

            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [Test]
        public async void ProjectAsyncCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            await sut.ProjectAsync(new object());

            Assert.That(mock.Commands, Is.Empty);
        }

        [Test]
        public async void ProjectAsyncTokenCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = await sut.ProjectAsync(new object(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [Test]
        public async void ProjectAsyncTokenCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = await sut.ProjectAsync(new object(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Commands, Is.Empty);
        }

        private static AsyncSqlProjector SutFactory()
        {
            return SutFactory(new SqlProjectionHandler[0], new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandler[] handlers)
        {
            return SutFactory(handlers, new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(IAsyncSqlNonQueryCommandExecutor executor)
        {
            return SutFactory(new SqlProjectionHandler[0], executor);
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandler[] handlers, IAsyncSqlNonQueryCommandExecutor executor)
        {
            return new AsyncSqlProjector(handlers, executor);
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("text", new DbParameter[0], CommandType.Text);
        }

        class ExecutorMock : IAsyncSqlNonQueryCommandExecutor
        {
            public readonly List<SqlNonQueryCommand> Commands = new List<SqlNonQueryCommand>();

            public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands)
            {
                return ExecuteNonQueryAsync(commands, CancellationToken.None);
            }

            public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken)
            {
                var count = Commands.Count;
                Commands.AddRange(commands);
                return Task.FromResult(Commands.Count - count);
            }
        }

        class ExecutorStub : IAsyncSqlNonQueryCommandExecutor
        {
            public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands)
            {
                return Task.FromResult(0);
            }

            public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
        }
    }
}