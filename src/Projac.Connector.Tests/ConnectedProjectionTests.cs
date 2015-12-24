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
                private readonly Signal _signalWhenWithoutCancellation;
                private readonly Signal _signalWhenWithCancellation;
                private readonly Signal _signalWhenSync;
                private readonly Signal[] _signals;

                public WithHandlers()
                {
                    _signalWhenWithoutCancellation = new Signal();
                    _signalWhenWithCancellation = new Signal();
                    _signalWhenSync = new Signal();
                    _signals = new []
                    {
                        _signalWhenWithoutCancellation, _signalWhenWithCancellation, _signalWhenSync
                    };
                    When<object>((_, __) => { _signalWhenWithoutCancellation.Set(); return TaskFactory(); });
                    When<object>((_, __, ___) => { _signalWhenWithCancellation.Set(); return TaskFactory(); });
                    When<object>((_, __) => { _signalWhenSync.Set(); });
                }

                public Signal[] Signals
                {
                    get { return _signals; }
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

                foreach (var _ in result)
                {
                    _.Handler(null, null, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                foreach (var _ in result)
                {
                    _.Handler(null, null, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }

            [Test]
            public void ImplicitConversionToSqlProjectionHandlerArray()
            {
                ConnectedProjectionHandler<object>[] result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(null, null, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }

            [Test]
            public void ExplicitConversionToSqlProjectionHandlerArray()
            {
                var result = (ConnectedProjectionHandler<object>[])_sut;

                foreach (var _ in result)
                {
                    _.Handler(null, null, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }
        }

        [TestFixture]
        public class WhenWithCancellationHandlerTests
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
        public class WhenWithoutCancellationHandlerTests
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

        [TestFixture]
        public class WhenSyncHandlerTests
        {
            [Test]
            public void WhenSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void WhenSyncHasExpectedResult()
            {
                var signal = new Signal();
                var handler = HandlerFactory(signal);
                var sut = new RegisterHandlers(handler);

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, CancellationToken.None);
                }

                Assert.That(signal.IsSet, Is.True);
            }

            [Test]
            public void SuccessiveWhenHasExpectedResult()
            {
                var signals = new List<Signal>();
                var handlers = new List<Action<object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    signals.Add(new Signal());
                    handlers.Add(HandlerFactory(signals[signals.Count - 1]));
                }
                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, CancellationToken.None);
                }

                Assert.That(signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }

            [Test]
            public void SuccessiveWhenRetainsOrder()
            {
                var signals = new List<Signal>();
                var handlers = new List<Action<object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    signals.Add(new Signal());
                    handlers.Add(HandlerFactory(signals[signals.Count - 1]));
                }
                signals.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, CancellationToken.None);
                }

                Assert.That(signals, Is.All.Matches<Signal>(signal => signal.IsSet));
            }

            private static readonly Random Random = new Random();

            private static Action<object, object> HandlerFactory(Signal signal)
            {
                return (_, __) => { signal.Set(); };
            }

            private class RegisterNullHandler : ConnectedProjection<object>
            {
                public RegisterNullHandler()
                {
                    When((Action<object, object>)null);
                }
            }

            private class RegisterHandlers : ConnectedProjection<object>
            {
                public RegisterHandlers(params Action<object, object>[] handlers)
                {
                    foreach (var handler in handlers)
                        When(handler);
                }
            }
        }
    }
}
