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
    namespace SqlProjection2Tests
    {
        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : SqlProjection2
            {
            }

            private WithoutHandlers _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithoutHandlers();
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
        public class SingleSqlNonQueryCommandReturningHandlerTests
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

            private class RegisterNullHandler : SqlProjection2
            {
                public RegisterNullHandler()
                {
                    When((Func<object, SqlNonQueryCommand>) null);
                }
            }

            private class RegisterHandlers : SqlProjection2
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

            private class RegisterNullHandler : SqlProjection2
            {
                public RegisterNullHandler()
                {
                    When((Func<object, SqlNonQueryCommand[]>) null);
                }
            }

            private class RegisterHandlers : SqlProjection2
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

            private class RegisterNullHandler : SqlProjection2
            {
                public RegisterNullHandler()
                {
                    When((Func<object, IEnumerable<SqlNonQueryCommand>>) null);
                }
            }

            private class RegisterHandlers : SqlProjection2
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
        public class AnyInstanceTests
        {
        }
    }

    [TestFixture]
    public class SqlProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SqlProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = SqlProjection.Empty;

            Assert.That(result, Is.InstanceOf<SqlProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(SqlProjection.Empty, SqlProjection.Empty);
        }

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjection) null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler4 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var projection = new SqlProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(projection);

            Assert.That(result.Handlers, Is.EquivalentTo(new[]{ handler1, handler2, handler3, handler4}));
        }

        [Test]
        public void ConcatHandlerReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(handler3);

            Assert.That(result.Handlers, Is.EquivalentTo(new[] { handler1, handler2, handler3 }));
        }

        [Test]
        public void ConcatHandlersReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler4 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(new[]
            {
                handler3,
                handler4
            });

            Assert.That(result.Handlers, Is.EquivalentTo(new[] { handler1, handler2, handler3, handler4 }));
        }

        [Test]
        public void EmptyToBuilderReturnsExpectedResult()
        {
            var sut = SqlProjection.Empty;
            
            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.EquivalentTo(new[]
            {
                handler1,
                handler2
            }));
        }

        [Test]
        public void ImplicitConversionToSqlProjectionHandlerArray()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            SqlProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToSqlProjectionHandlerArray()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            var result = (SqlProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}