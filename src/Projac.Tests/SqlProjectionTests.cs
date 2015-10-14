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
    namespace SqlProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : SqlProjection
            {
            }

            private SqlProjection _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any();
            }

            [Test]
            public void IsEnumerableOfSqlProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<SqlProjectionHandler>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : SqlProjection
            {
            }

            private SqlProjection _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithoutHandlers();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                Assert.That(_sut, Is.Empty);
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                Assert.That(_sut.Handlers, Is.Empty);
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                SqlProjectionHandler[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (SqlProjectionHandler[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : SqlProjection
            {
                private readonly SqlNonQueryCommand _command;
                private readonly SqlNonQueryCommand[] _commandArray;
                private readonly IEnumerable<SqlNonQueryCommand> _commandEnumeration;
                private readonly SqlNonQueryCommand[] _result;

                public WithHandlers()
                {
                    _command = CommandFactory();
                    _commandArray = new[]
                    {
                        CommandFactory(), CommandFactory()
                    };
                    _commandEnumeration = Enumerable.Repeat(CommandFactory(), 1);

                    var commands = new List<SqlNonQueryCommand>();
                    commands.Add(_command);
                    commands.AddRange(_commandArray);
                    commands.AddRange(_commandEnumeration);
                    _result = commands.ToArray();

                    When<object>(m => _command);
                    When<object>(m => _commandArray);
                    When<object>(m => _commandEnumeration);
                }

                public SqlNonQueryCommand[] Result
                {
                    get { return _result; }
                }

                private static SqlNonQueryCommand CommandFactory()
                {
                    return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
                }
            }

            private WithHandlers _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithHandlers();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<SqlProjectionHandler> result = _sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)), 
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                SqlProjectionHandler[] result = _sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (SqlProjectionHandler[])_sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_sut.Result));
            }
        }

        [TestFixture]
        public class SqlNonQueryCommandReturningHandlerTests
        {
            [Test]
            public void WhenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void WhenHasExpectedResult()
            {
                var command = CommandFactory();
                var handler = HandlerFactory(command);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EquivalentTo(new[] {command}));
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, SqlNonQueryCommand>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    commands.Add(CommandFactory());
                    handlers.Add(HandlerFactory(commands[commands.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EquivalentTo(commands));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, SqlNonQueryCommand>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    commands.Add(CommandFactory());
                    handlers.Add(HandlerFactory(commands[commands.Count - 1]));
                }
                commands.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EquivalentTo(commands));
            }

            private static readonly Random Random = new Random();

            private static Func<object, SqlNonQueryCommand> HandlerFactory(SqlNonQueryCommand command)
            {
                return o => command;
            }

            private static SqlNonQueryCommand CommandFactory()
            {
                return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
            }

            private class RegisterNullHandler : SqlProjection
            {
                public RegisterNullHandler()
                {
                    When((Func<object, SqlNonQueryCommand>) null);
                }
            }

            private class RegisterHandlers : SqlProjection
            {
                public RegisterHandlers(params Func<object, SqlNonQueryCommand>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }
        }

        [TestFixture]
        public class SqlNonQueryCommandArrayReturningHandlerTests
        {
            [Test]
            public void WhenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void WhenHasExpectedResult()
            {
                var command1 = CommandFactory();
                var command2 = CommandFactory();
                var handler = HandlerFactory(command1, command2);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EquivalentTo(new[] {command1, command2}));
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, SqlNonQueryCommand[]>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    var handlerCommands = RandomCommandSet();
                    commands.AddRange(handlerCommands);
                    handlers.Add(HandlerFactory(handlerCommands.ToArray()));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(commands));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, SqlNonQueryCommand[]>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    var handlerCommands = RandomCommandSet();
                    commands.InsertRange(0, handlerCommands);
                    handlers.Add(HandlerFactory(handlerCommands.ToArray()));
                }
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(commands));
            }

            private static readonly Random Random = new Random();

            private static List<SqlNonQueryCommand> RandomCommandSet()
            {
                var handlerCommands = new List<SqlNonQueryCommand>();
                for (var commandCount = 0; commandCount < Random.Next(2, 10); commandCount++)
                {
                    handlerCommands.Add(CommandFactory());
                }
                return handlerCommands;
            }

            private class RegisterNullHandler : SqlProjection
            {
                public RegisterNullHandler()
                {
                    When((Func<object, SqlNonQueryCommand[]>) null);
                }
            }

            private class RegisterHandlers : SqlProjection
            {
                public RegisterHandlers(params Func<object, SqlNonQueryCommand[]>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }

            private static Func<object, SqlNonQueryCommand[]> HandlerFactory(params SqlNonQueryCommand[] commands)
            {
                return o => commands;
            }

            private static SqlNonQueryCommand CommandFactory()
            {
                return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
            }
        }

        [TestFixture]
        public class SqlNonQueryCommandEnumerationReturningHandlerTests
        {
            [Test]
            public void WhenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void WhenHasExpectedResult()
            {
                var command1 = CommandFactory();
                var command2 = CommandFactory();
                var handler = HandlerFactory(command1, command2);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EquivalentTo(new[] {command1, command2}));
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, IEnumerable<SqlNonQueryCommand>>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    var handlerCommands = RandomCommandSet();
                    commands.AddRange(handlerCommands);
                    handlers.Add(HandlerFactory(handlerCommands.ToArray()));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(commands));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var commands = new List<SqlNonQueryCommand>();
                var handlers = new List<Func<object, IEnumerable<SqlNonQueryCommand>>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    var handlerCommands = RandomCommandSet();
                    commands.InsertRange(0, handlerCommands);
                    handlers.Add(HandlerFactory(handlerCommands.ToArray()));
                }
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(sut.Handlers.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(commands));
            }

            private static readonly Random Random = new Random();

            private static List<SqlNonQueryCommand> RandomCommandSet()
            {
                var handlerCommands = new List<SqlNonQueryCommand>();
                for (var commandCount = 0; commandCount < Random.Next(2, 10); commandCount++)
                {
                    handlerCommands.Add(CommandFactory());
                }
                return handlerCommands;
            }

            private class RegisterNullHandler : SqlProjection
            {
                public RegisterNullHandler()
                {
                    When((Func<object, IEnumerable<SqlNonQueryCommand>>) null);
                }
            }

            private class RegisterHandlers : SqlProjection
            {
                public RegisterHandlers(params Func<object, IEnumerable<SqlNonQueryCommand>>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }

            private static Func<object, SqlNonQueryCommand[]> HandlerFactory(params SqlNonQueryCommand[] commands)
            {
                return o => commands;
            }

            private static SqlNonQueryCommand CommandFactory()
            {
                return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
            }
        }
    }
}