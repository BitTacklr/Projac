using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    namespace ProjectionWithMetadataTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : Projection<object, object>
            {
            }

            private Projection<object, object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any();
            }

            [Test]
            public void IsEnumerableOfProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<object, object>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : Projection<object, object>
            {
            }

            private Projection<object, object> _sut;

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
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<object, object>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object, object>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : Projection<object, object>
            {
                private readonly Signal<object> _signalHandleWithoutCancellation;
                private readonly Signal<object> _signalHandleWithCancellation;
                private readonly Signal<object> _signalHandleSync;
                private readonly Signal<object>[] _signals;

                public WithHandlers()
                {
                    _signalHandleWithoutCancellation = new Signal<object>();
                    _signalHandleWithCancellation = new Signal<object>();
                    _signalHandleSync = new Signal<object>();
                    _signals = new []
                    {
                        _signalHandleWithoutCancellation, _signalHandleWithCancellation, _signalHandleSync
                    };
                    Handle<object>((_, __, metadata) => { _signalHandleWithoutCancellation.Set(metadata); return TaskFactory(); });
                    Handle<object>((_, __, metadata, ____) => { _signalHandleWithCancellation.Set(metadata); return TaskFactory(); });
                    Handle<object>((_, __, metadata) => { _signalHandleSync.Set(metadata); });
                }

                public Signal<object>[] Signals
                {
                    get { return _signals; }
                }

                private static Task TaskFactory()
                {
                    return Task.CompletedTask;
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
                IEnumerable<ProjectionHandler<object, object>> result = _sut;
                var metadata = new object();

                foreach (var _ in result)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;
                var metadata = new object();

                foreach (var _ in result)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<object, object>[] result = _sut;
                var metadata = new object();                

                foreach (var _ in result)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object, object>[])_sut;
                var metadata = new object();                

                foreach (var _ in result)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }
                Assert.That(_sut.Signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }
        }

        [TestFixture]
        public class HandleWithCancellationHandlerTests
        {
            [Test]
            public void HandleHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleHasExpectedResult()
            {
                var task = TaskFactory();
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, object, CancellationToken, Task> HandlerFactory(Task task)
            {
                return (_, __, ___, ____) => task;
            }

            private static Task TaskFactory()
            {
                return Task.CompletedTask;
            }

            private class RegisterNullHandler : Projection<object, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<object, object, object, CancellationToken, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<object, object>
            {
                public RegisterHandlers(params Func<object, object, object, CancellationToken, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleWithoutCancellationHandlerTests
        {
            [Test]
            public void HandleHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleHasExpectedResult()
            {
                var task = TaskFactory();
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<object, object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory());
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                Assert.That(
                    sut.Handlers.Select(_ => _.Handler(null, null, null, CancellationToken.None)),
                    Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, object, Task> HandlerFactory(Task task)
            {
                return (_, __, ___) => task;
            }

            private static Task TaskFactory()
            {
                return Task.CompletedTask;
            }

            private class RegisterNullHandler : Projection<object, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<object, object, object, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<object, object>
            {
                public RegisterHandlers(params Func<object, object, object, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleSyncHandlerTests
        {
            [Test]
            public void HandleSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleSyncHasExpectedResult()
            {
                var signal = new Signal<object>();
                var handler = HandlerFactory(signal);
                var metadata = new object();
                var sut = new RegisterHandlers(handler);

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }

                Assert.That(signal.IsSet, Is.True);
                Assert.That(signal.Metadata, Is.EqualTo(metadata));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var signals = new List<Signal<object>>();
                var metadata = new object();
                var handlers = new List<Action<object, object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    signals.Add(new Signal<object>());
                    handlers.Add(HandlerFactory(signals[signals.Count - 1]));
                }
                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }

                Assert.That(signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var signals = new List<Signal<object>>();
                var metadata = new object();
                var handlers = new List<Action<object, object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    signals.Add(new Signal<object>());
                    handlers.Add(HandlerFactory(signals[signals.Count - 1]));
                }
                signals.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(null, null, metadata, CancellationToken.None);
                }

                Assert.That(signals, Is.All.Matches<Signal<object>>(signal => signal.IsSet && signal.Metadata == metadata));
            }

            private static readonly Random Random = new Random();

            private static Action<object, object, object> HandlerFactory(Signal<object> signal)
            {
                return (_, __, metadata) => { signal.Set(metadata); };
            }

            private class RegisterNullHandler : Projection<object, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Action<object, object, object>)null);
                }
            }

            private class RegisterHandlers : Projection<object, object>
            {
                public RegisterHandlers(params Action<object, object, object>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }
    }
}
