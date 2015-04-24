using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using Paramol;
using Paramol.Executors;
using Projac.Tests.Framework;

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
                () => SutFactory((ISqlNonQueryCommandExecutor)null));
            
        }

        [Test]
        public void ProjectMessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Project((object)null));
        }

        [Test]
        public void ProjectMessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Project((IEnumerable<object>)null));
        }

        [Test]
        public void ProjectMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands = new[] {CommandFactory(), CommandFactory()};
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(object), _ => commands);
            var sut = SutFactory(new[] {handler}, mock);

            var result = sut.Project(new object());

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [Test]
        public void ProjectMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches2()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(Envelope), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(Envelope<Message>), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = sut.Project(new MessageEnvelope<Message2>());

            Assert.That(result, Is.EqualTo(4));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1.Concat(commands2)));
        }

        interface Message { }
        interface Message1 : Message { }
        interface Message2 : Message { }
        interface Envelope { }
        interface Envelope<out TMessage> : Envelope { TMessage Message { get; } }

        class MessageEnvelope<TMessage> : Envelope<TMessage>
        {
            public TMessage Message
            {
                get { return default(TMessage); }
            }
        }


        [Test]
        public void ProjectMessageCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = sut.Project(new object());

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Commands, Is.Empty);
        }

        [Test]
        public void ProjectMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = sut.Project(new object[] { 123, true });

            Assert.That(result, Is.EqualTo(4));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1.Concat(commands2)));
        }

        [Test]
        public void ProjectMessagesCausesExecutorToBeCalledWithExpectedCommandsWhenMessageTypeMismatches()
        {
            var commands1 = new[] { CommandFactory(), CommandFactory() };
            var commands2 = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler1 = new SqlProjectionHandler(typeof(int), _ => commands1);
            var handler2 = new SqlProjectionHandler(typeof(bool), _ => commands2);
            var sut = SutFactory(new[] { handler1, handler2 }, mock);

            var result = sut.Project(new object[] { 123 });

            Assert.That(result, Is.EqualTo(2));
            Assert.That(mock.Commands, Is.EquivalentTo(commands1));
        }

        private static SqlProjector SutFactory()
        {
            return SutFactory(new SqlProjectionHandler[0], new ExecutorStub());
        }

        private static SqlProjector SutFactory(SqlProjectionHandler[] handlers)
        {
            return SutFactory(handlers, new ExecutorStub());
        }

        private static SqlProjector SutFactory(ISqlNonQueryCommandExecutor executor)
        {
            return SutFactory(new SqlProjectionHandler[0], executor);
        }

        private static SqlProjector SutFactory(SqlProjectionHandler[] handlers, ISqlNonQueryCommandExecutor executor)
        {
            return new SqlProjector(handlers, executor);
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("text", new DbParameter[0], CommandType.Text);
        }

        class ExecutorMock : ISqlNonQueryCommandExecutor
        {
            public readonly List<SqlNonQueryCommand> Commands = new List<SqlNonQueryCommand>();

            public void ExecuteNonQuery(SqlNonQueryCommand command)
            {
                Commands.Add(command);
            }

            public int ExecuteNonQuery(IEnumerable<SqlNonQueryCommand> commands)
            {
                var count = Commands.Count;
                Commands.AddRange(commands);
                return Commands.Count - count;
            }
        }

        class ExecutorStub : ISqlNonQueryCommandExecutor
        {
            public void ExecuteNonQuery(SqlNonQueryCommand command)
            {
            }

            public int ExecuteNonQuery(IEnumerable<SqlNonQueryCommand> commands)
            {
                return 0;
            }
        }
    }
}
