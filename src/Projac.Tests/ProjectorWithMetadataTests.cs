using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class ProjectorWithMetadataTests
    {
        [Test]
        public void ResolverCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((ProjectionHandlerResolver<object, object>)null));
        }

        [Test]
        public void ProjectAsync_ConnectionCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync((object)null, new object(), new object()));
        }

        [Test]
        public void ProjectAsync_MessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(new object(), (object)null, new object()));
        }

        [Test]
        public void ProjectAsync_MetadataCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync(new object(), new object(), (object)null));
        }

        [Test]
        public void ProjectAsyncToken_ConnectionCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync((object)null, new object(), CancellationToken.None));
        }

        [Test]
        public void ProjectAsyncToken_MessageCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(new object(), (object)null, CancellationToken.None));
        }

        [Test]
        public void ProjectAsyncToken_MetadataCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync(new object(), new object(), (object)null, CancellationToken.None));
        }

        [Test]
        public void ProjectAsyncMany_ConnectionCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync((object)null, new (object, object)[0]));
        }

        [Test]
        public void ProjectAsyncMany_MessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ProjectAsync(new object(), (IEnumerable<(object, object)>)null));
        }

        [Test]
        public void ProjectAsyncManyToken_ConnectionCanBeNull()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ProjectAsync((object)null, new (object, object)[0], CancellationToken.None));
        }

        [Test]
        public void ProjectAsyncManyToken_MessagesCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.ProjectAsync(new object(), (IEnumerable<(object, object)>) null, CancellationToken.None));
        }

        [TestCaseSource(typeof(ProjectorWithMetadataProjectCases), nameof(ProjectorWithMetadataProjectCases.ProjectMessageWithoutTokenCases))]
        public async Task ProjectAsyncMessageCausesExpectedCalls(
            ProjectionHandlerResolver<CallRecordingConnectionWithMetadata, object> resolver,
            object message,
            object metadata,
            Tuple<int, object, object, CancellationToken>[] expectedCalls)
        {
            var connection = new CallRecordingConnectionWithMetadata();
            var sut = SutFactory(resolver);

            await sut.ProjectAsync(connection, message, metadata);

            Assert.That(connection.RecordedCalls, Is.EquivalentTo(expectedCalls));
        }

        [TestCaseSource(typeof(ProjectorWithMetadataProjectCases), nameof(ProjectorWithMetadataProjectCases.ProjectMessageWithTokenCases))]
        public async Task ProjectAsyncTokenMessageCausesExpectedCalls(
            ProjectionHandlerResolver<CallRecordingConnectionWithMetadata, object> resolver,
            object message,
            object metadata,
            CancellationToken token,
            Tuple<int, object, object, CancellationToken>[] expectedCalls)
        {
            var connection = new CallRecordingConnectionWithMetadata();
            var sut = SutFactory(resolver);

            await sut.ProjectAsync(connection, message, metadata, token);

            Assert.That(connection.RecordedCalls, Is.EquivalentTo(expectedCalls));
        }

        [TestCaseSource(typeof(ProjectorWithMetadataProjectCases), nameof(ProjectorWithMetadataProjectCases.ProjectMessagesWithoutTokenCases))]
        public async Task ProjectAsyncMessagesCausesExpectedCalls(
            ProjectionHandlerResolver<CallRecordingConnectionWithMetadata, object> resolver,
            (object, object)[] messages,
            Tuple<int, object, object, CancellationToken>[] expectedCalls)
        {
            var connection = new CallRecordingConnectionWithMetadata();
            var sut = SutFactory(resolver);

            await sut.ProjectAsync(connection, messages);

            Assert.That(connection.RecordedCalls, Is.EquivalentTo(expectedCalls));
        }

        [TestCaseSource(typeof(ProjectorWithMetadataProjectCases), nameof(ProjectorWithMetadataProjectCases.ProjectMessagesWithTokenCases))]
        public async Task ProjectAsyncTokenMessagesCausesExpectedCalls(
            ProjectionHandlerResolver<CallRecordingConnectionWithMetadata, object> resolver,
            (object, object)[] messages,
            CancellationToken token,
            Tuple<int, object, object, CancellationToken>[] expectedCalls)
        {
            var connection = new CallRecordingConnectionWithMetadata();
            var sut = SutFactory(resolver);

            await sut.ProjectAsync(connection, messages, token);

            Assert.That(connection.RecordedCalls, Is.EquivalentTo(expectedCalls));
        }

        [Test]
        public void ProjectAsyncMessageResolverFailureCausesExpectedResult()
        {
            ProjectionHandlerResolver<object, object> resolver = m =>
            {
                throw new Exception("message");
            };
            var sut = SutFactory(resolver);
            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new object(), new object()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessageResolverFailureCausesExpectedResult()
        {
            ProjectionHandlerResolver<object, object> resolver = m =>
            {
                throw new Exception("message");
            };
            var sut = SutFactory(resolver);
            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new object(), new object(), new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessagesResolverFailureCausesExpectedResult()
        {
            ProjectionHandlerResolver<object, object> resolver = m =>
            {
                throw new Exception("message");
            };
            var sut = SutFactory(resolver);
            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new object(), new object()) }),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessagesResolverFailureCausesExpectedResult()
        {
            ProjectionHandlerResolver<object, object> resolver = m =>
            {
                throw new Exception("message");
            };
            var sut = SutFactory(resolver);
            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new object(), new object()) }, new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessageFirstHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = m => new[] {new ProjectionHandler<object, object>(typeof (object), handler)};
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new object(), new object()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessageSecondHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new int(), new object()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessageSecondHandlerCancellationCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetCanceled();
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new int(), new object()),
                Throws.TypeOf<TaskCanceledException>());
        }

        [Test]
        public void ProjectAsyncTokenMessageFirstHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = m => new[] { new ProjectionHandler<object, object>(typeof(object), handler) };
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new object(), new object(), new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessageSecondHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new int(), new object(), new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessageSecondHandlerCancellationCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetCanceled();
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new int(), new object(), new CancellationToken()),
                Throws.TypeOf<TaskCanceledException>());
        }


        [Test]
        public void ProjectAsyncMessagesFirstHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = m => new[] { new ProjectionHandler<object, object>(typeof(object), handler) };
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new object(), new object()) }),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessagesSecondHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new int(), new object()) }),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncMessagesSecondHandlerCancellationCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetCanceled();
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new int(), new object()) }),
                Throws.TypeOf<TaskCanceledException>());
        }

        [Test]
        public void ProjectAsyncTokenMessagesFirstHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = m => new[] { new ProjectionHandler<object, object>(typeof(object), handler) };
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new object(), new object()) }, new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessagesSecondHandlerFailureCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetException(new Exception("message"));
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new int(), new object()) }, new CancellationToken()),
                Throws.TypeOf<Exception>().And.Message.EqualTo("message"));
        }

        [Test]
        public void ProjectAsyncTokenMessagesSecondHandlerCancellationCausesExpectedResult()
        {
            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    source.SetCanceled();
                    return source.Task;
                };
            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(int), handler2)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new int(), new object()) }, new CancellationToken()),
                Throws.TypeOf<TaskCanceledException>());
        }

        [Test]
        public void ProjectAsyncTokenMessagesHandlerObservingCancelledTokenCausesExpectedResult()
        {
            var tcs = new CancellationTokenSource();

            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) =>
                {
                    var source = new TaskCompletionSource<object>();
                    token.Register(() => source.SetCanceled());

                    tcs.Cancel();
                    return source.Task;
                };

            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
            });
            var sut = SutFactory(resolver);
            
            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()) }, tcs.Token),
                Throws.TypeOf<TaskCanceledException>());
        }

        [Test]
        public void ProjectAsyncTokenMessagesCancellationAfterSecondHandlerCausesExpectedResult()
        {
            var tcs = new CancellationTokenSource();

            Func<object, object, object, CancellationToken, Task> handler1 =
                (connection, message, metadata, token) => Task.CompletedTask;
            Func<object, object, object, CancellationToken, Task> handler2 =
                (connection, message, metadata, token) =>
                {
                    tcs.Cancel();
                    return Task.CompletedTask;
                };
            Func<object, object, object, CancellationToken, Task> handler3 =
                (connection, message, metadata, token) =>
                {
                    throw new InvalidOperationException("Should not happen!");
                };

            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
                new ProjectionHandler<object, object>(typeof(object), handler2),
                new ProjectionHandler<object, object>(typeof(object), handler3)
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()), (new object(), new object()), (new object(), new object()) }, tcs.Token),
                Throws.Nothing);
        }

        [Test]
        public void ProjectAsyncTokenMessagesCancellationAfterSecondHandlerInContinuationCausesExpectedResult()
        {
            var tcs = new CancellationTokenSource();
            tcs.Cancel();

            Func<object, object, object, CancellationToken, Task> handler1 =
                async (connection, message, metadata, token) =>
                {
                    await Task.Yield();
                    await Task.Delay(TimeSpan.FromDays(1), token);
                };

            ProjectionHandlerResolver<object, object> resolver = Resolve.WhenEqualToHandlerMessageType(new[]
            {
                new ProjectionHandler<object, object>(typeof(object), handler1),
            });
            var sut = SutFactory(resolver);

            Assert.That(async () =>
                await sut.ProjectAsync(new object(), new (object, object)[] { (new object(), new object()) }, tcs.Token),
                Throws.TypeOf<TaskCanceledException>());
        }

        private static Projector<object, object> SutFactory()
        {
            return SutFactory(message => new ProjectionHandler<object, object>[0]);
        }

        private static Projector<TConnection, TMetadata> SutFactory<TConnection, TMetadata>(ProjectionHandlerResolver<TConnection, TMetadata> resolver)
        {
            return new Projector<TConnection, TMetadata>(resolver);
        }
    }
}
