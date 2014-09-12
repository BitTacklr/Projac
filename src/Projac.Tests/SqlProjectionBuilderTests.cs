using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using Paramol;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionBuilderTests
    {
        private SqlProjectionBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new SqlProjectionBuilder();
        }

        [Test]
        public void InitialInstanceBuildReturnsExpectedResult()
        {
            var result = _sut.Build();

            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void WhenHandlerWithSingleCommandCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, SqlNonQueryCommand>)null));
        }


        [Test]
        public void WhenHandlerWithSingleCommandReturnsExpectedResult()
        {
            var result = _sut.When((object _) => CommandFactory());

            Assert.That(result, Is.InstanceOf<SqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithSingleCommandIsPreservedUponBuild()
        {
            var command = CommandFactory();
            Func<object, SqlNonQueryCommand> handler = _ => command;
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command })), 
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithSingleStatementPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var commands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var command = CommandFactory();
            Func<object, SqlNonQueryCommand> handler = _ => command;
            var result = _sut.When((object _) => commands).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithCommandArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, SqlNonQueryCommand[]>)null));
        }

        [Test]
        public void WhenHandlerWithCommandArrayReturnsExpectedResult()
        {
            var result = _sut.When((object _) => new[] { CommandFactory(), CommandFactory() });

            Assert.That(result, Is.InstanceOf<SqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementArrayIsPreservedUponBuild()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, SqlNonQueryCommand[]> handler = _ => new[] { command1, command2 };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command1, command2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementArrayPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var commands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, SqlNonQueryCommand[]> handler = _ => new[] { command1, command2 };
            var result = _sut.When((object _) => commands).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithCommandEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, IEnumerable<SqlNonQueryCommand>>)null));
        }

        [Test]
        public void WhenHandlerWithCommandEnumerationReturnsExpectedResult()
        {
            var result = _sut.When((object _) => (IEnumerable<SqlNonQueryCommand>) new []
            {
                CommandFactory(), CommandFactory()
            });

            Assert.That(result, Is.InstanceOf<SqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationIsPreservedUponBuild()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, IEnumerable<SqlNonQueryCommand>> handler = _ => (IEnumerable<SqlNonQueryCommand>) new []
            {
                command1, command2
            };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command1, command2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithCommandEnumerationPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var commands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, IEnumerable<SqlNonQueryCommand>> handler = _ => (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            };
            var result = _sut.When((object _) => commands).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("text", new DbParameter[0], CommandType.Text);
        }
    }
}
