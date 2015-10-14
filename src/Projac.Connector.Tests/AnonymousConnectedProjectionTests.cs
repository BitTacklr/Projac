using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    namespace AnonymousConnectedProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : AnonymousConnectedProjection<object>
            {
                public Any(ConnectedProjectionHandler<object>[] handlers)
                    : base(handlers)
                {
                }
            }

            private AnonymousConnectedProjection<object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any(new ConnectedProjectionHandler<object>[0]);
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AnonymousConnectedProjection<object>(null));
            }

            [Test]
            public void IsEnumerableOfSqlProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ConnectedProjectionHandler<object>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : AnonymousConnectedProjection<object>
            {
                public WithoutHandlers()
                    : base(new ConnectedProjectionHandler<object>[0])
                {
                }
            }

            private AnonymousConnectedProjection<object> _sut;

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
                ConnectedProjectionHandler<object>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ConnectedProjectionHandler<object>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : AnonymousConnectedProjection<object>
            {
                public WithHandlers(ConnectedProjectionHandler<object>[] handlers)
                    : base(handlers)
                {
                }
            }

            private static Task TaskFactory()
            {
                return Task.FromResult<object>(null);
            }

            private static ConnectedProjectionHandler<object> HandlerFactory(Task task)
            {
                return new ConnectedProjectionHandler<object>(typeof(object), (_, __, ___) => task);
            }

            private WithHandlers _sut;
            private Task _task1;
            private Task _task2;
            private Task[] _result;

            [SetUp]
            public void SetUp()
            {
                _task1 = TaskFactory();
                _task2 = TaskFactory();
                _result = new [] {_task1, _task2};

                _sut = new WithHandlers(new[]
                {
                    HandlerFactory(_task1),
                    HandlerFactory(_task2),
                });
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ConnectedProjectionHandler<object>> result = _sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                ConnectedProjectionHandler<object>[] result = _sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ConnectedProjectionHandler<object>[])_sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }
        }
    }
}
