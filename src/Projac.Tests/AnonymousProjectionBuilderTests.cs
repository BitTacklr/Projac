using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    namespace AnonymousProjectionBuilderTests
    {
        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            private AnonymousProjectionBuilder<CallRecordingConnection> _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private CancellationToken _token;

            [SetUp]
            public void SetUp()
            {
                _sut = new AnonymousProjectionBuilder<CallRecordingConnection>();
                _connection = new CallRecordingConnection();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void BuildReturnsExpectedResult()
            {
                var result = _sut.Build();
                Assert.That(result, Is.InstanceOf<AnonymousProjection<CallRecordingConnection>>());
                Assert.That(result.Handlers, Is.Empty);
            }

            [Test]
            public void HandleWithCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, CancellationToken, Task>)null));
            }

            [Test]
            public void HandleWithCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, CancellationToken, Task> handler = (connection, message, token) => 
                {
                    connection.RecordCall(message, token);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message, _token) }));
                Assert.That(result, Is.EquivalentTo(new [] { task }));
            }

            [Test]
            public void HandleWithoutCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, Task>)null));
            }

            [Test]
            public void HandleWithoutCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, Task> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { task }));
            }

            [Test]
            public void HandleSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Action<CallRecordingConnection, object>)null));
            }

            [Test]
            public void HandleSyncHasExpectedResult()
            {
                Action<CallRecordingConnection, object> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { Task.CompletedTask }));
            }
        }

        [TestFixture]
        public class InstanceWithInjectedHandlersTests
        {
            private AnonymousProjectionBuilder<CallRecordingConnection> _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private CancellationToken _token;
            private Task _task1;
            private Task _task2;

            [SetUp]
            public void SetUp()
            {
                _task1 = Task.FromResult<object>(new object());
                _task2 = Task.FromResult<object>(new object());
                _sut = new AnonymousProjectionBuilder<CallRecordingConnection>(new []
                {
                    new ProjectionHandler<CallRecordingConnection>(typeof(object), (connection, message, token) => 
                    {
                        connection.RecordCall(message, token);
                        return _task1;
                    }),
                    new ProjectionHandler<CallRecordingConnection>(typeof(object), (connection, message, token) => 
                    {
                        connection.RecordCall(message, token);
                        return _task2;
                    })
                });
                _connection = new CallRecordingConnection();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void BuildReturnsExpectedResult()
            {
                var projection = _sut.Build();
                Assert.That(projection, Is.InstanceOf<AnonymousProjection<CallRecordingConnection>>());

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message, _token), new RecordedCall(_message, _token) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2 }));
            }

            [Test]
            public void HandleWithCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, CancellationToken, Task>)null));
            }

            [Test]
            public void HandleWithCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, CancellationToken, Task> handler = (connection, message, token) => 
                {
                    connection.RecordCall(message, token);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message, _token), new RecordedCall(_message, _token), new RecordedCall(_message, _token) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, task }));
            }

            [Test]
            public void HandleWithoutCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, Task>)null));
            }

            [Test]
            public void HandleWithoutCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, Task> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message, _token), new RecordedCall(_message, _token), new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, task }));
            }

            [Test]
            public void HandleSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Action<CallRecordingConnection, object>)null));
            }

            [Test]
            public void HandleSyncHasExpectedResult()
            {
                Action<CallRecordingConnection, object> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message, _token), new RecordedCall(_message, _token), new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, Task.CompletedTask }));
            }
        }

        [TestFixture]
        public class InstanceWithHandlersTests
        {
            private AnonymousProjectionBuilder<CallRecordingConnection> _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private CancellationToken _token;
            private Task _task1;
            private Task _task2;

            [SetUp]
            public void SetUp()
            {
                _task1 = Task.FromResult<object>(new object());
                _task2 = Task.FromResult<object>(new object());
                _sut = new AnonymousProjectionBuilder<CallRecordingConnection>()
                    .Handle<object>((connection, message) => 
                    {
                        connection.RecordCall(message);
                        return _task1;
                    })
                    .Handle<object>((connection, message, token) => 
                    {
                        connection.RecordCall(message, token);
                        return _task2;
                    });
                _connection = new CallRecordingConnection();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void BuildReturnsExpectedResult()
            {
                var projection = _sut.Build();
                Assert.That(projection, Is.InstanceOf<AnonymousProjection<CallRecordingConnection>>());

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message), new RecordedCall(_message, _token) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2 }));
            }

            [Test]
            public void HandleWithCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, CancellationToken, Task>)null));
            }

            [Test]
            public void HandleWithCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, CancellationToken, Task> handler = (connection, message, token) => 
                {
                    connection.RecordCall(message, token);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message), new RecordedCall(_message, _token), new RecordedCall(_message, _token) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, task }));
            }

            [Test]
            public void HandleWithoutCancellationTokenHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Func<CallRecordingConnection, object, Task>)null));
            }

            [Test]
            public void HandleWithoutCancellationTokenHasExpectedResult()
            {
                var task = Task.FromResult<object>(new object());
                Func<CallRecordingConnection, object, Task> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                    return task;
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message), new RecordedCall(_message, _token), new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, task }));
            }

            [Test]
            public void HandleSyncHandlerCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(() => _sut.Handle((Action<CallRecordingConnection, object>)null));
            }

            [Test]
            public void HandleSyncHasExpectedResult()
            {
                Action<CallRecordingConnection, object> handler = (connection, message) => 
                {
                    connection.RecordCall(message);
                };
                
                _sut = _sut.Handle(handler);

                var result = _sut.Build().Select(_ => _.Handler(_connection, _message, _token)).ToArray();
                Assert.That(_connection.RecordedCalls, Is.EquivalentTo(new [] { new RecordedCall(_message), new RecordedCall(_message, _token), new RecordedCall(_message) }));
                Assert.That(result, Is.EquivalentTo(new [] { _task1, _task2, Task.CompletedTask }));
            }
        }
    }
}
