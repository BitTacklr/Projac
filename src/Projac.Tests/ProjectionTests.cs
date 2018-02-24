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
            class Any : Projection<CallRecordingConnection>
            {
            }

            private Projection<CallRecordingConnection> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any();
            }

            [Test]
            public void IsEnumerableOfProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<CallRecordingConnection>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : Projection<CallRecordingConnection>
            {
            }

            private Projection<CallRecordingConnection> _sut;

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
                ProjectionHandler<CallRecordingConnection>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : Projection<CallRecordingConnection>
            {
                public WithHandlers()
                {
                    Handle((CallRecordingConnection connection, object message) => 
                    {
                        connection.RecordCall(message);
                        return Task.CompletedTask;
                    });
                    Handle((CallRecordingConnection connection, object message, CancellationToken token) => 
                    {
                        connection.RecordCall(message, token);
                        return Task.CompletedTask;
                    });
                    Handle((CallRecordingConnection connection,object  message) =>
                    {
                        connection.RecordCall(message);
                    });
                }
            }

            private WithHandlers _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _sut = new WithHandlers();
                _connection = new CallRecordingConnection();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ProjectionHandler<CallRecordingConnection>> result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message },
                    new object[] { _message, _token },
                    new object[] { _message }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message },
                    new object[] { _message, _token },
                    new object[] { _message }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<CallRecordingConnection>[] result = _sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message },
                    new object[] { _message, _token },
                    new object[] { _message }
                }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection>[])_sut;

                foreach (var _ in result)
                {
                    _.Handler(_connection, _message, _token);
                }
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new []
                {
                    new object[] { _message },
                    new object[] { _message, _token },
                    new object[] { _message }
                }).Using(new RecordedCallEqualityComparer()));
            }
        }

        [TestFixture]
        public class HandleWithCancellationHandlerTests
        {
            private CallRecordingConnection _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
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
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, CancellationToken, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message, _token }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<CallRecordingConnection, object, CancellationToken, Task> HandlerFactory(Task task)
            {
                return (connection, message, token) => 
                {
                    connection.RecordCall(message, token);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<CallRecordingConnection, object, CancellationToken, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection>
            {
                public RegisterHandlers(params Func<CallRecordingConnection, object, CancellationToken, Task>[] handlers)
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
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
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
                var handler = HandlerFactory(task);

                var sut = new RegisterHandlers(handler);

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(new[] { task }));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var tasks = new List<Task>();
                var handlers = new List<Func<CallRecordingConnection, object, Task>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    tasks.Add(TaskFactory(new object()));
                    handlers.Add(HandlerFactory(tasks[tasks.Count - 1]));
                }
                tasks.Reverse();
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                var result = sut.Handlers.Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
                Assert.That(result, Is.EquivalentTo(tasks));
            }

            private static readonly Random Random = new Random();

            private static Func<CallRecordingConnection, object, Task> HandlerFactory(Task task)
            {
                return (connection, message) => 
                {
                    connection.RecordCall(message);
                    return task;
                };
            }

            private static Task TaskFactory(object result)
            {
                return Task.FromResult<object>(result);
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection>
            {
                public RegisterNullHandler()
                {
                    Handle((Func<CallRecordingConnection, object, Task>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection>
            {
                public RegisterHandlers(params Func<CallRecordingConnection, object, Task>[] handlers)
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
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _connection = new CallRecordingConnection();
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
                var handler = HandlerFactory();
                var sut = new RegisterHandlers(handler);

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void SuccessiveHandleHasExpectedResult()
            {
                var handlers = new List<Action<CallRecordingConnection, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    handlers.Add(HandlerFactory());
                }
                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
            }

            [Test]
            public void SuccessiveHandleRetainsOrder()
            {
                var handlers = new List<Action<CallRecordingConnection, object>>();
                for (var index = 0; index < Random.Next(2, 100); index++)
                {
                    handlers.Add(HandlerFactory());
                }
                handlers.Reverse();

                var sut = new RegisterHandlers(handlers.ToArray());

                foreach (var _ in sut.Handlers)
                {
                    _.Handler(_connection, _message, _token);
                }

                Assert.That(
                    _connection.RecordedCalls, 
                    Is.All.EqualTo(new object[] { _message }).Using(new RecordedCallEqualityComparer()));
            }

            private static readonly Random Random = new Random();

            private static Action<CallRecordingConnection, object> HandlerFactory()
            {
                return (connection, message) => { connection.RecordCall(message); };
            }

            private class RegisterNullHandler : Projection<CallRecordingConnection>
            {
                public RegisterNullHandler()
                {
                    Handle((Action<CallRecordingConnection, object>)null);
                }
            }

            private class RegisterHandlers : Projection<CallRecordingConnection>
            {
                public RegisterHandlers(params Action<CallRecordingConnection, object>[] handlers)
                {
                    foreach (var handler in handlers)
                        Handle(handler);
                }
            }
        }
    }
}
