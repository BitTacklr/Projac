using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    namespace AnonymousProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : AnonymousProjection<object>
            {
                public Any(ProjectionHandler<object>[] handlers)
                    : base(handlers)
                {
                }
            }

            private AnonymousProjection<object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any(new ProjectionHandler<object>[0]);
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AnonymousProjection<object>(null));
            }

            [Test]
            public void IsEnumerableOfSqlProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<object>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : AnonymousProjection<object>
            {
                public WithoutHandlers()
                    : base(new ProjectionHandler<object>[0])
                {
                }
            }

            private AnonymousProjection<object> _sut;

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
                ProjectionHandler<object>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : AnonymousProjection<object>
            {
                public WithHandlers(ProjectionHandler<object>[] handlers)
                    : base(handlers)
                {
                }
            }

            private static Task TaskFactory()
            {
                return Task.FromResult<object>(null);
            }

            private static ProjectionHandler<object> HandlerFactory(Task task)
            {
                return new ProjectionHandler<object>(typeof(object), (_, __, ___) => task);
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
                IEnumerable<ProjectionHandler<object>> result = _sut;

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
                ProjectionHandler<object>[] result = _sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object>[])_sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_result));
            }
        }
    }
}
