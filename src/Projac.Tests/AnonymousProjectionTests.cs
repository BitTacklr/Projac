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
            class Any : AnonymousProjection<CallRecordingConnection>
            {
                public Any(ProjectionHandler<CallRecordingConnection>[] handlers)
                    : base(handlers)
                {
                }
            }

            private AnonymousProjection<CallRecordingConnection> _sut;

            [SetUp]
            public void SetUp()
            {
                _sut = new Any(new ProjectionHandler<CallRecordingConnection>[0]);
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
                Assert.That(_sut, Is.AssignableTo<IEnumerable<ProjectionHandler<CallRecordingConnection>>>());
            }
        }

        [TestFixture]
        public class InstanceWithoutHandlersTests
        {
            class WithoutHandlers : AnonymousProjection<CallRecordingConnection>
            {
                public WithoutHandlers()
                    : base(new ProjectionHandler<CallRecordingConnection>[0])
                {
                }
            }

            private AnonymousProjection<CallRecordingConnection> _sut;

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
            class WithHandlers : AnonymousProjection<CallRecordingConnection>
            {
                public WithHandlers(Task[] tasks) : base(new [] 
                {
                    new ProjectionHandler<CallRecordingConnection>(typeof(object), (CallRecordingConnection connection, object message, CancellationToken token) => 
                    {
                        connection.RecordCall(1, message, token);
                        return tasks[0];
                    }),
                    new ProjectionHandler<CallRecordingConnection>(typeof(object), (CallRecordingConnection connection, object message, CancellationToken token) => 
                    {
                        connection.RecordCall(2, message, token);
                        return tasks[1];
                    })
                }) {}
            }

            private AnonymousProjection<CallRecordingConnection> _sut;
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
                _sut = new WithHandlers(new [] { _task1, _task2 });
                _connection = new CallRecordingConnection();
                _message = new object();
                _token = new CancellationToken();
            }

            [Test]
            public void GetEnumeratorReturnsExpectedInstance()
            {
                IEnumerable<ProjectionHandler<CallRecordingConnection>> result = _sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void HandlersReturnsExpectedResult()
            {
                var result = _sut.Handlers;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void ImplicitConversionToProjectionHandlerArray()
            {
                ProjectionHandler<CallRecordingConnection>[] result = _sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }

            [Test]
            public void ExplicitConversionToProjectionHandlerArray()
            {
                var result = (ProjectionHandler<CallRecordingConnection>[])_sut;

                var tasks = result.Select(_ => _.Handler(_connection, _message, _token));
                Assert.That(_connection.RecordedCalls, Is.All.EqualTo(new RecordedCall(_message, _token)));
                Assert.That(tasks, Is.EquivalentTo(new Task[] { _task1, _task2 }));
            }
        }
    }
}
