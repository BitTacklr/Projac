using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paramol;
using Paramol.Executors;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectorTests
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

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessageCases")]
        public void ProjectMessageCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object message,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = sut.Project(message);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        [TestCaseSource(typeof(ProjectorProjectCases), "ProjectMessagesCases")]
        public void ProjectMessagesCausesExecutorToBeCalledWithExpectedCommands(
            SqlProjectionHandlerResolver resolver,
            object[] messages,
            SqlNonQueryCommand[] commands)
        {
            var mock = new ExecutorMock();
            var sut = SutFactory(resolver, mock);

            var result = sut.Project(messages);

            Assert.That(result, Is.EqualTo(commands.Length));
            Assert.That(mock.Commands, Is.EquivalentTo(commands));
        }

        private static SqlProjector SutFactory()
        {
            return SutFactory(Resolve.WhenEqualToHandlerMessageType(new SqlProjectionHandler[0]), new ExecutorStub());
        }

        private static SqlProjector SutFactory(SqlProjectionHandlerResolver resolver)
        {
            return SutFactory(resolver, new ExecutorStub());
        }

        private static SqlProjector SutFactory(ISqlNonQueryCommandExecutor executor)
        {
            return SutFactory(Resolve.WhenEqualToHandlerMessageType(new SqlProjectionHandler[0]), executor);
        }

        private static SqlProjector SutFactory(SqlProjectionHandlerResolver resolver, ISqlNonQueryCommandExecutor executor)
        {
            return new SqlProjector(resolver, executor);
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
