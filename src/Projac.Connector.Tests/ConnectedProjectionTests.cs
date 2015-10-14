using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    namespace ConnectedProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : ConnectedProjection<object>
            {
            }

            private ConnectedProjection<object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any();
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
            class WithoutHandlers : ConnectedProjection<object>
            {
            }

            private ConnectedProjection<object> _sut;

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
            class WithHandlers : ConnectedProjection<object>
            {
                private readonly Task _taskWithoutCancellation;
                private readonly Task _taskWithCancellation;
                private readonly Task[] _result;

                public WithHandlers()
                {
                    _taskWithoutCancellation = TaskFactory();
                    _taskWithCancellation = TaskFactory();
                    _result = new[] { _taskWithoutCancellation, _taskWithCancellation };

                    When<object>((_, __) => _taskWithoutCancellation);
                    When<object>((_, __, ___) => _taskWithCancellation);
                }

                public Task[] Result
                {
                    get { return _result; }
                }

                private static Task TaskFactory()
                {
                    return Task.FromResult<object>(null);
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
                IEnumerable<ConnectedProjectionHandler<object>> result = _sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                ConnectedProjectionHandler<object>[] result = _sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_sut.Result));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ConnectedProjectionHandler<object>[])_sut;

                Assert.That(result.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EqualTo(_sut.Result));
            }
        }

        [TestFixture]
        public class WithCancellationHandlerTests
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
                var task = TaskFactory();
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, CancellationToken, Task> HandlerFactory(Task task)
            {
                return (_, __, ___) => task;
            }

            private static Task TaskFactory()
            {
                return Task.FromResult<object>(null);
            }

            private class RegisterNullHandler : ConnectedProjection<object>
            {
                public RegisterNullHandler()
                {
                    When((Func<object, object, CancellationToken, Task>)null);
                }
            }

            private class RegisterHandlers : ConnectedProjection<object>
            {
                public RegisterHandlers(params Func<object, object, CancellationToken, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }
        }

        [TestFixture]
        public class WithoutCancellationHandlerTests
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
                var task = TaskFactory();
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, Task> HandlerFactory(Task task)
            {
                return (_, __) => task;
            }

            private static Task TaskFactory()
            {
                return Task.FromResult<object>(null);
            }

            private class RegisterNullHandler : ConnectedProjection<object>
            {
                public RegisterNullHandler()
                {
                    When((Func<object, object, Task>)null);
                }
            }

            private class RegisterHandlers : ConnectedProjection<object>
            {
                public RegisterHandlers(params Func<object, object, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }
        }
    }
}
