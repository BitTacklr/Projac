using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Paramol;
using Paramol.Executors;

namespace Projac.Tests
{
    [TestFixture]
    public class AsyncSqlProjectorTests
    {
        [Test]
        public void ResolverCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((SqlProjectionHandlerResolver)null));
        }

        [Test]
        public void ExecutorCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((IAsyncSqlNonQueryCommandExecutor)null));
        }

        [Test]
        public void ProjectAsync_MessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((object)null));
        }

        [Test]
        public void ProjectAsyncToken_MessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((object)null, CancellationToken.None));
        }

        [Test]
        public void ProjectAsync_MessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((IEnumerable<object>)null));
        }

        [Test]
        public void ProjectAsyncToken_MessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((IEnumerable<object>)null, CancellationToken.None));
        }

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessageCases")]
        public async Task ProjectAsyncMessageCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object message,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = await sut.ProjectAsync(message);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessagesCases")]
        public async Task ProjectAsyncMessagesCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object[] messages,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = await sut.ProjectAsync(messages);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessageCases")]
        public async Task ProjectAsyncMessageTokenCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object message,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = await sut.ProjectAsync(message, CancellationToken.None);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessagesCases")]
        public async Task ProjectAsyncMessagesTokenCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object[] messages,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = await sut.ProjectAsync(messages, CancellationToken.None);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        private static AsyncSqlProjector SutFactory()
        {
            return SutFactory(message => new SqlProjectionHandler[0], new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandlerResolver resolver)
        {
            return SutFactory(resolver, new ExecutorStub());
        }

        private static AsyncSqlProjector SutFactory(IAsyncSqlNonQueryCommandExecutor executor)
        {
            return SutFactory(message => new SqlProjectionHandler[0], executor);
        }

        private static AsyncSqlProjector SutFactory(SqlProjectionHandlerResolver resolver, IAsyncSqlNonQueryCommandExecutor executor)
        {
            return new AsyncSqlProjector(resolver, executor);
        }

        class ExecutorMock : IAsyncSqlNonQueryCommandExecutor
        {
            public readonly List<SqlNonQueryCommand> Commands = new List<SqlNonQueryCommand>();

            public Task ExecuteNonQueryAsync(SqlNonQueryCommand command)
            {
                return ExecuteNonQueryAsync(command, CancellationToken.None);
            }

            public Task ExecuteNonQueryAsync(SqlNonQueryCommand command, CancellationToken cancellationToken)
            {
                Commands.Add(command);
                return Task.FromResult<object>(null);
            }

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
            public Task ExecuteNonQueryAsync(SqlNonQueryCommand command)
            {
                return Task.FromResult<object>(null);
            }

            public Task ExecuteNonQueryAsync(SqlNonQueryCommand command, CancellationToken cancellationToken)
            {
                return Task.FromResult<object>(null);
            }

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