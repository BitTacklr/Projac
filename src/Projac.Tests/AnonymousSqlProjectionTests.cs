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
    namespace AnonymousSqlProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : AnonymousSqlProjection
            {
                public Any(SqlProjectionHandler[] handlers) : base(handlers)
                {
                }
            }

            private AnonymousSqlProjection _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any(new SqlProjectionHandler[0]);
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AnonymousSqlProjection(null));
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
            class WithoutHandlers : AnonymousSqlProjection
            {
                public WithoutHandlers() : base(new SqlProjectionHandler[0])
                {
                }
            }

            private AnonymousSqlProjection _sut;

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
            class WithHandlers : AnonymousSqlProjection
            {
                public WithHandlers(SqlProjectionHandler[] handlers) : base(handlers)
                {
                }
            }

            private static SqlNonQueryCommand CommandFactory()
            {
                return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
            }

            private static SqlProjectionHandler HandlerFactory(SqlNonQueryCommand command)
            {
                return new SqlProjectionHandler(typeof(object), o => new[] {command});
            }

            private static SqlProjectionHandler HandlerFactory(SqlNonQueryCommand[] commands)
            {
                return new SqlProjectionHandler(typeof(object), o => commands);
            }

            private WithHandlers _sut;
            private SqlNonQueryCommand _command;
            private SqlNonQueryCommand[] _commandArray;
            private SqlNonQueryCommand[] _result;

            [SetUp]
            public void SetUp()
            {
                _command = CommandFactory();
                _commandArray = new[]
                    {
                        CommandFactory(), CommandFactory()
                    };

                var commands = new List<SqlNonQueryCommand>();
                commands.Add(_command);
                commands.AddRange(_commandArray);
                _result = commands.ToArray();

                _sut = new WithHandlers(new []
                {
                    HandlerFactory(_command),
                    HandlerFactory(_commandArray),
                });
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<SqlProjectionHandler> result = _sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                SqlProjectionHandler[] result = _sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (SqlProjectionHandler[])_sut;

                Assert.That(result.SelectMany(_ => _.Handler(null)),
                    Is.EqualTo(_result));
            }
        }
    }
}
