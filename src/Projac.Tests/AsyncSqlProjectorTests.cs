using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
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
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((object)null));
        }

        [Test]
        public void ProjectAsyncTokenMessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((object)null, CancellationToken.None));
        }

        [Test]
        public void ProjectAsyncMessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((IEnumerable<object>)null));
        }

        [Test]
        public void ProjectAsyncTokenMessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync((IEnumerable<object>)null, CancellationToken.None));
        }

        [Test]
        public async void ProjectAsyncMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = await sut.ProjectAsync(new object());

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [Test]
        public async void ProjectAsyncMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = await sut.ProjectAsync(new object[] { 123, true });

            Assert.That(result, Is.EqualTo(4));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1.Concat(commands2)));
        }

        [Test]
        public async void ProjectAsyncMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = await sut.ProjectAsync(new object());

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Commands, Is.Empty);
        }

        [Test]
        public async void ProjectAsyncMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = await sut.ProjectAsync(new object[] { 123 });

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1));
        }

        [Test]
        public async void ProjectAsyncTokenMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
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
        public async void ProjectAsyncTokenMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = await sut.ProjectAsync(new object[] { 123, true }, CancellationToken.None);

            Assert.That(result, Is.EqualTo(4));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1.Concat(commands2)));
        }

        [Test]
        public async void ProjectAsyncTokenMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = await sut.ProjectAsync(new object(), CancellationToken.None);

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Commands, Is.Empty);
        }

        [Test]
        public async void ProjectAsyncTokenMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = await sut.ProjectAsync(new object[] { 123 }, CancellationToken.None);

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1));
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