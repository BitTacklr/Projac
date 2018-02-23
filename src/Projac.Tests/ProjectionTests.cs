using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    namespace ProjectionTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : Projection<object>
            {
            }

            private Projection<object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any();
            }

            [Test]
            public void IsEnumerableOfProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<object>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : Projection<object>
            {
            }

            private Projection<object> _sut;

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
                ProjectionHandler<object>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : Projection<object>
            {
                public WithHandlers()
                {
                    HandleWithoutCancellation = new MethodCallSink();
                    HandleWithCancellation = new MethodCallSink();
                    HandleSync = new MethodCallSink();
                    Handle((object connection, object message) => 
                    {
                        HandleWithoutCancellation.Capture(connection, message);
                        return Task.CompletedTask;
                    });
                    Handle((object connection, object message, CancellationToken token) => 
                    {
                        HandleWithCancellation.Capture(connection, message, token);
                        return Task.CompletedTask;
                    });
                    Handle((object connection,object  message) =>
                    {
                        HandleSync.Capture(connection, message);
                    });
                }

                public MethodCallSink HandleWithoutCancellation { get; private set; }

                public MethodCallSink HandleWithCancellation { get; private set; }

                public MethodCallSink HandleSync { get; private set; }
            }

            private WithHandlers _sut;
            private object _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithHandlers();
                _connection = new object();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ProjectionHandler<object>> result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_sut.HandleSync.Matches(_connection, _message), Is.True);
                Assert.That(_sut.HandleWithCancellation.Matches(_connection, _message, _token), Is.True);
                Assert.That(_sut.HandleWithoutCancellation.Matches(_connection, _message), Is.True);
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_sut.HandleSync.Matches(_connection, _message), Is.True);
                Assert.That(_sut.HandleWithCancellation.Matches(_connection, _message, _token), Is.True);
                Assert.That(_sut.HandleWithoutCancellation.Matches(_connection, _message), Is.True);
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<object>[] result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_sut.HandleSync.Matches(_connection, _message), Is.True);
                Assert.That(_sut.HandleWithCancellation.Matches(_connection, _message, _token), Is.True);
                Assert.That(_sut.HandleWithoutCancellation.Matches(_connection, _message), Is.True);
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<object>[])_sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_sut.HandleSync.Matches(_connection, _message), Is.True);
                Assert.That(_sut.HandleWithCancellation.Matches(_connection, _message, _token), Is.True);
                Assert.That(_sut.HandleWithoutCancellation.Matches(_connection, _message), Is.True);
            }
        }

        [TestFixture]
        public class HandleWithCancellationHandlerTests
        {
            private object _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new object();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void HandleHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleHasExpectedResult()
            {
                var task = TaskFactory(new object());
                var sink = new MethodCallSink();
                var handler = HandlerFactory(sink, task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(sink.Matches(_connection, _message, _token), Is.True);
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Func<object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1], tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();

                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message, _token)));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Func<object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1], tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message, _token)));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, CancellationToken, Task> HandlerFactory(MethodCallSink sink, Task task)
            {
                return (connection, message, token) => 
                {
                    sink.Capture(connection, message, token);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<object, object, CancellationToken, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<object>
            {
                public RegisterHandlers(params Func<object, object, CancellationToken, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleWithoutCancellationHandlerTests
        {
            private object _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new object();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void HandleHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleHasExpectedResult()
            {
                var task = TaskFactory(new object());
                var sink = new MethodCallSink();
                var handler = HandlerFactory(sink, task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(sink.Matches(_connection, _message), Is.True);
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Func<object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1], tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message)));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Func<object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1], tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message)));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<object, object, Task> HandlerFactory(MethodCallSink sink, Task task)
            {
                return (connection, message) => 
                {
                    sink.Capture(connection, message);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<object, object, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<object>
            {
                public RegisterHandlers(params Func<object, object, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleSyncHandlerTests
        {
            private object _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new object();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void HandleSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new RegisterNullHandler());
            }

            [Test]
            public void HandleSyncHasExpectedResult()
            {
                var sink = new MethodCallSink();
                var handler = HandlerFactory(sink);
                var sut = new RegisterHandlers(handler);

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(sink.Matches(_connection, _message), Is.True);
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Action<object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1]));
                }
                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message)));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var sinks = new List<MethodCallSink>();
                var handlers = new List<Action<object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    sinks.Add(new MethodCallSink());
                    handlers.Add(HandlerFactory(sinks[sinks.Count - 1]));
                }
                sinks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(sinks, Is.All.Matches<MethodCallSink>(sink => sink.Matches(_connection, _message)));
            }

            private static readonly Random Random = new Random();

            private static Action<object, object> HandlerFactory(MethodCallSink sink)
            {
                return (connection, message) => { sink.Capture(connection, message); };
            }

            private class RegisterNullHandler : Projection<object>
            {
                public RegisterNullHandler()
                {
                    Handle((Action<object, object>)null);
                }
            }

            private class RegisterHandlers : Projection<object>
            {
                public RegisterHandlers(params Action<object, object>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }
    }
}
