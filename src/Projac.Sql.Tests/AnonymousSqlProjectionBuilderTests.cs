using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using Projac.Sql.Tests.Framework;

namespace Projac.Sql.Tests
{
    [TestFixture]
    public class AnonymousSqlProjectionBuilderTests
    {
        private AnonymousSqlProjectionBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new AnonymousSqlProjectionBuilder();
        }

        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AnonymousSqlProjectionBuilder(null));
        }

        [Test]
        public void HandlersAreCopiedOnConstruction()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), o => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), o => new SqlNonQueryCommand[0]);
            var sut = new AnonymousSqlProjectionBuilder(new[]
            {
                handler1, 
                handler2
            });

            var result = sut.Build();

            Assert.That(result, Is.EquivalentTo(new[]
            {
                handler1, handler2
            }));

        }

        [Test]
        public void EmptyInstanceBuildReturnsExpectedResult()
        {
            var result = new AnonymousSqlProjectionBuilder().Build();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void InitialInstanceBuildReturnsExpectedResult()
        {
            var result = _sut.Build();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void HandleHandlerWithSingleCommandCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<object, SqlNonQueryCommand>)null));
        }


        [Test]
        public void HandleHandlerWithSingleCommandReturnsExpectedResult()
        {
            var result = _sut.Handle((object _) => CommandFactory());

            Assert.That(result, Is.InstanceOf<AnonymousSqlProjectionBuilder>());
        }

        [Test]
        public void HandleHandlerWithSingleCommandIsPreservedUponBuild()
        {
            var command = CommandFactory();
            Func<object, SqlNonQueryCommand> handler = _ => command;
            var result = _sut.Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command })),
                Is.EqualTo(1));
        }

        [Test]
        public void HandleHandlerWithSingleStatementPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var commands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var command = CommandFactory();
            Func<object, SqlNonQueryCommand> handler = _ => command;
            var result = _sut.Handle((object _) => commands).Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        [Test]
        public void HandleHandlerWithCommandArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<object, SqlNonQueryCommand[]>)null));
        }

        [Test]
        public void HandleHandlerWithCommandArrayReturnsExpectedResult()
        {
            var result = _sut.Handle((object _) => new[] { CommandFactory(), CommandFactory() });

            Assert.That(result, Is.InstanceOf<AnonymousSqlProjectionBuilder>());
        }

        [Test]
        public void HandleHandlerWithStatementArrayIsPreservedUponBuild()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, SqlNonQueryCommand[]> handler = _ => new[] { command1, command2 };
            var result = _sut.Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command1, command2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void HandleHandlerWithStatementArrayPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var commands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, SqlNonQueryCommand[]> handler = _ => new[] { command1, command2 };
            var result = _sut.Handle((object _) => commands).Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        [Test]
        public void HandleHandlerWithCommandEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<object, IEnumerable<SqlNonQueryCommand>>)null));
        }

        [Test]
        public void HandleHandlerWithCommandEnumerationReturnsExpectedResult()
        {
            var result = _sut.Handle((object _) => (IEnumerable<SqlNonQueryCommand>)new[]
            {
                CommandFactory(), CommandFactory()
            });

            Assert.That(result, Is.InstanceOf<AnonymousSqlProjectionBuilder>());
        }

        [Test]
        public void HandleHandlerWithStatementEnumerationIsPreservedUponBuild()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            Func<object, IEnumerable<SqlNonQueryCommand>> handler = _ => (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            };
            var result = _sut.Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(new[] { command1, command2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void HandleHandlerWithCommandEnumerationPreservesPreviouslyCollectedStatementsUponBuild()
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
            var result = _sut.Handle((object _) => commands).Handle(handler).Build();

            Assert.That(
                result.Count(_ => _.Message == typeof(object) && _.Handler(null).SequenceEqual(commands)),
                Is.EqualTo(1));
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("text", new DbParameter[0], CommandType.Text);
        }
    }
}