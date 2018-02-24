using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    namespace AnonymousProjectionWithMetadataTests
    {
        [TestFixture]
        public class AnyInstanceTests
        {
            class Any : AnonymousProjection<CallRecordingConnection, object>
            {
                public Any(ProjectionHandler<CallRecordingConnection, object>[] handlers)
                    : base(handlers)
                {
                }
            }

            private AnonymousProjection<CallRecordingConnection, object> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any(new ProjectionHandler<CallRecordingConnection, object>[0]);
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AnonymousProjection<object>(null));
            }

            [Test]
            public void IsEnumerableOfProjectionHandler()
            {
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<CallRecordingConnection, object>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : AnonymousProjection<CallRecordingConnection, object>
            {
                public WithoutHandlers()
                    : base(new ProjectionHandler<CallRecordingConnection, object>[0])
                {
                }
            }

            private AnonymousProjection<CallRecordingConnection, object> _sut;

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
                ProjectionHandler<CallRecordingConnection, object>[] result = _sut;

                Assert.That(result, Is.Empty);
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection, object>[])_sut;

                Assert.That(result, Is.Empty);
            }
        }

         [TestFixture]
        public class InstanceWithHandlersTests
        {
            class WithHandlers : AnonymousProjection<CallRecordingConnection, object>
            {
                public WithHandlers(Task[] tasks) : base(new [] 
                {
                    new ProjectionHandler<CallRecordingConnection, object>(typeof(object), (CallRecordingConnection connection, object message, object metadata, CancellationToken token) => 
                    {
                        connection.RecordCall(1, message, metadata, token);
                        return tasks[0];
                    }),
                    new ProjectionHandler<CallRecordingConnection, object>(typeof(object), (CallRecordingConnection connection, object message, object metadata, CancellationToken token) => 
                    {
                        connection.RecordCall(2, message, metadata, token);
                        return tasks[1];
                    })
                }) {}
            }

            private AnonymousProjection<CallRecordingConnection, object> _sut;
            private CallRecordingConnection _connection;
            private object _message;
            private object _metadata;
            private CancellationToken _token;
            private Task _task1;
            private Task _task2;

            [SetUp]
            public void SetUp()
            {
                _task1 = Task.FromResult<object>(new object());
                _task2 = Task.FromResult<object>(new object());
                _sut = new WithHandlers(new [] { _task1, _task2 });
                _connection = new CallRecordingConnection();
                _message = new object();
                _metadata = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ProjectionHandler<CallRecordingConnection, object>> result = _sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _metadata, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _metadata, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _metadata, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _metadata, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<CallRecordingConnection, object>[] result = _sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _metadata, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _metadata, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection, object>[])_sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _metadata, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _metadata, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }
        }
    }
}
