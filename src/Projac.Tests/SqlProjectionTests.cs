using System;
using System.Collections;
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

        [TestFixture]
        public class EnumeratorTests
        {
            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new SqlProjection.Enumerator(null));
            }

            [Test]
            public void DisposeDoesNotThrow()
            {
                Assert.DoesNotThrow(() => 
                    new SqlProjection.Enumerator(new SqlProjectionHandler[0]));
            }

            [TestCaseSource("MoveNextCases")]
            public void MoveNextReturnsExpectedResult(
                SqlProjection.Enumerator sut, bool expected)
            {
                var result = sut.MoveNext();
                Assert.That(result, Is.EqualTo(expected));
            }

            private static IEnumerable<TestCaseData> MoveNextCases()
            {
                //No handlers
                var enumerator1 = new SqlProjection.Enumerator(new SqlProjectionHandler[0]);
                yield return new TestCaseData(enumerator1, false);
                yield return new TestCaseData(enumerator1, false); //idempotency check

                //1 handler
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator2, true);
                yield return new TestCaseData(enumerator2, false);
                yield return new TestCaseData(enumerator2, false); //idempotency check

                //2 handlers
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator3, true);
                yield return new TestCaseData(enumerator3, true);
                yield return new TestCaseData(enumerator3, false);
                yield return new TestCaseData(enumerator3, false); //idempotency check
            }

            [TestCaseSource("MoveNextAfterResetCases")]
            public void MoveNextAfterResetReturnsExpectedResult(
                SqlProjection.Enumerator sut, bool expected)
            {
                sut.Reset();

                var result = sut.MoveNext();

                Assert.That(result, Is.EqualTo(expected));
            }

            private static IEnumerable<TestCaseData> MoveNextAfterResetCases()
            {
                //No handlers
                var enumerator1 = new SqlProjection.Enumerator(new SqlProjectionHandler[0]);
                yield return new TestCaseData(enumerator1, false);
                yield return new TestCaseData(enumerator1, false);

                //1 handler
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator2, true);
                yield return new TestCaseData(enumerator2, true);

                //2 handlers
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator3, true);
                yield return new TestCaseData(enumerator3, true);
            }

            [TestCaseSource("ResetCases")]
            public void ResetDoesNotThrow(SqlProjection.Enumerator sut)
            {
                Assert.DoesNotThrow(sut.Reset);
            }

            private static IEnumerable<TestCaseData> ResetCases()
            {
                //No handlers
                var enumerator1 = new SqlProjection.Enumerator(new SqlProjectionHandler[0]);
                yield return new TestCaseData(enumerator1);

                //1 handler
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator2);

                //2 handlers
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator3);
            }

            [TestCaseSource("CurrentNotStartedCases")]
            public void CurrentReturnsExpectedResultWhenNotStarted(
                SqlProjection.Enumerator sut)
            {
                Assert.Throws<InvalidOperationException>(
                    () => { var _ = sut.Current; });
            }

            [TestCaseSource("CurrentNotStartedCases")]
            public void EnumeratorCurrentReturnsExpectedResultWhenNotStarted(
                IEnumerator sut)
            {
                Assert.Throws<InvalidOperationException>(
                    () => { var _ = sut.Current; });
            }

            private static IEnumerable<TestCaseData> CurrentNotStartedCases()
            {
                //No handlers
                var enumerator1 = new SqlProjection.Enumerator(new SqlProjectionHandler[0]);
                yield return new TestCaseData(enumerator1);

                //1 handler
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator2);

                //2 handlers
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
                yield return new TestCaseData(enumerator3);
            }

            [TestCaseSource("CurrentCompletedCases")]
            public void CurrentReturnsExpectedResultWhenCompleted(
                SqlProjection.Enumerator sut)
            {
                Assert.Throws<InvalidOperationException>(
                    () => { var _ = sut.Current; });
            }

            [TestCaseSource("CurrentCompletedCases")]
            public void EnumeratorCurrentReturnsExpectedResultWhenCompleted(
                IEnumerator sut)
            {
                Assert.Throws<InvalidOperationException>(
                    () => { var _ = sut.Current; });
            }

            private static IEnumerable<TestCaseData> CurrentCompletedCases()
            {
                //No handlers
                var enumerator1 = new SqlProjection.Enumerator(new SqlProjectionHandler[0]);
                while (enumerator1.MoveNext()) { }
                yield return new TestCaseData(enumerator1);

                //1 handler
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory())
                });
                while (enumerator2.MoveNext()) { }
                yield return new TestCaseData(enumerator2);

                //2 handlers
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(CommandFactory()),
                    HandlerFactory(CommandFactory())
                });
                while (enumerator3.MoveNext()) { }
                yield return new TestCaseData(enumerator3);
            }

            [TestCaseSource("CurrentStartedCases")]
            public void CurrentReturnsExpectedResultWhenStarted(
                SqlProjection.Enumerator sut, SqlNonQueryCommand[] expected)
            {
                sut.MoveNext();

                var result = sut.Current.Handler(null);

                Assert.That(result, Is.EqualTo(expected));
            }

            [TestCaseSource("CurrentStartedCases")]
            public void EnumeratorCurrentReturnsExpectedResultWhenStarted(
                IEnumerator sut, SqlNonQueryCommand[] expected)
            {
                sut.MoveNext();

                var result = ((SqlProjectionHandler)sut.Current).Handler(null);

                Assert.That(result, Is.EqualTo(expected));
            }

            private static IEnumerable<TestCaseData> CurrentStartedCases()
            {
                //No handlers - not applicable

                //1 handler
                var command1 = CommandFactory();
                var enumerator2 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(command1)
                });
                yield return new TestCaseData(enumerator2, new[]
                {
                    command1
                });

                //2 handlers
                var command2 = CommandFactory();
                var command3 = CommandFactory();
                var enumerator3 = new SqlProjection.Enumerator(new[]
                {
                    HandlerFactory(command2),
                    HandlerFactory(command3)
                });
                yield return new TestCaseData(enumerator3, new[]
                {
                    command2
                });
                yield return new TestCaseData(enumerator3, new[]
                {
                    command3
                });
            }

            private static SqlProjectionHandler HandlerFactory(SqlNonQueryCommand command)
            {
                return new SqlProjectionHandler(
                    typeof (object),
                    o => new[] {command});
            }

            private static SqlNonQueryCommand CommandFactory()
            {
                return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
            }
        }
    }
}