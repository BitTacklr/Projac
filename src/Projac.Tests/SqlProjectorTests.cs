using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Paramol;
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
        public void ProjectEventCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Project(null));
        }

        [Test]
        public void ProjectCausesExecutorToBeCalledWithExpectedCommandsWhenEventTypeMatches()
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
        public void ProjectCausesExecutorToBeCalledWithExpectedCommandsWhenEventTypeMismatches()
        {
            var commands = new[] { CommandFactory(), CommandFactory() };
            var mock = new ExecutorMock();
            var handler = new SqlProjectionHandler(typeof(string), _ => commands);
            var sut = SutFactory(new[] { handler }, mock);

            var result = sut.Project(new object());

            Assert.That(result, Is.EqualTo(0));
            Assert.That(mock.Commands, Is.Empty);
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

            public int Execute(IEnumerable<SqlNonQueryCommand> commands)
            {
                var count = Commands.Count;
                Commands.AddRange(commands);
                return Commands.Count - count;
            }
        }

        class ExecutorStub : ISqlNonQueryCommandExecutor
        {
            public int Execute(IEnumerable<SqlNonQueryCommand> commands)
            {
                return 0;
            }
        }
    }
}
