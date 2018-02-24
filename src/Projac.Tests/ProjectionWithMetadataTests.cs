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
            class WithHandlers : Projection<CallRecordingConnection, object>
            {
                public WithHandlers()
                {
                    Handle((CallRecordingConnection connection, object message, object metadata) => 
                    {
                        connection.RecordCall(message, metadata);
                        return Task.CompletedTask;
                    });
                    Handle((CallRecordingConnection connection, object message, object metadata, CancellationToken token) => 
                    {
                        connection.RecordCall(message, metadata, token);
                        return Task.CompletedTask;
                    });
                    Handle((CallRecordingConnection connection, object  message, object metadata) =>
                    {
                        connection.RecordCall(message, metadata);
                    });
                }
            }

            private WithHandlers _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private object _metadata;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithHandlers();
                _connection = new CallRecordingConnection();
                _message = new object();
                _metadata = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ProjectionHandler<CallRecordingConnection, object>> result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message, _metadata },
                    new object[] { _message, _metadata, _token },
                    new object[] { _message, _metadata }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message, _metadata },
                    new object[] { _message, _metadata, _token },
                    new object[] { _message, _metadata }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<CallRecordingConnection, object>[] result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }

                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message, _metadata },
                    new object[] { _message, _metadata, _token },
                    new object[] { _message, _metadata }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection, object>[])_sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message, _metadata },
                    new object[] { _message, _metadata, _token },
                    new object[] { _message, _metadata }
                }).Using(new RecordedCallEqualityComparer()));
            }
        }

        [TestFixture]
        public class HandleWithCancellationHandlerTests
        {
            private CallRecordingConnection _connection;
            private object _message;
            private object _metadata;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
                _message = new object();
                _metadata = new object();
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
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<CallRecordingConnection, object, object, CancellationToken, Task> HandlerFactory(Task task)
            {
                return (connection, message, metadata, token) => 
                {
                    connection.RecordCall(message, metadata, token);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<CallRecordingConnection, object, object, CancellationToken, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection, object>
            {
                public RegisterHandlers(params Func<CallRecordingConnection, object, object, CancellationToken, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleWithoutCancellationHandlerTests
        {
            private CallRecordingConnection _connection;
            private object _message;
            private object _metadata;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
                _message = new object();
                _metadata = new object();
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
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _metadata, _token)).ToArray();
                
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<CallRecordingConnection, object, object, Task> HandlerFactory(Task task)
            {
                return (connection, message, metadata) => 
                {
                    connection.RecordCall(message, metadata);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<CallRecordingConnection, object, object, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection, object>
            {
                public RegisterHandlers(params Func<CallRecordingConnection, object, object, Task>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }

        [TestFixture]
        public class HandleSyncHandlerTests
        {
            private CallRecordingConnection _connection;
            private object _message;
            private object _metadata;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
                _message = new object();
                _metadata = new object();
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
                var handler = HandlerFactory();
                var sut = new RegisterHandlers(handler);

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var handlers = new List<Action<CallRecordingConnection, object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    handlers.Add(HandlerFactory());
                }
                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var handlers = new List<Action<CallRecordingConnection, object, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    handlers.Add(HandlerFactory());
                }
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _metadata, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _metadata }).Using(new RecordedCallEqualityComparer()));
            }

            private static readonly Random Random = new Random();

            private static Action<CallRecordingConnection, object, object> HandlerFactory()
            {
                return (connection, message, metadata) => { connection.RecordCall(message, metadata); };
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection, object>
            {
                public RegisterNullHandler()
                {
                    Handle((Action<CallRecordingConnection, object, object>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection, object>
            {
                public RegisterHandlers(params Action<CallRecordingConnection, object, object>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }
    }
}
