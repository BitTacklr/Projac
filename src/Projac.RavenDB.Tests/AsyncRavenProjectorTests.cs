using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Client;
using Raven.Client.Embedded;

namespace Projac.RavenDB.Tests
{
    namespace AsyncRavenProjectorTests
    {
        [TestFixture]
        public class GuardTests
        {
            private EmbeddableDocumentStore _store;
            private IAsyncDocumentSession _session;

            [SetUp]
            public void SetUp()
            {
                _store = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    DataDirectory = Path.GetTempPath()
                };
                _store.Initialize();
                _session = _store.OpenAsyncSession();
            }

            [TearDown]
            public void TearDown()
            {
                _session.Dispose();
                _store.Dispose();
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AsyncRavenProjector(null));
            }

            [Test]
            public void ProjectAsync_ClientCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(null, new object()));
            }

            [Test]
            public void ProjectAsync_MessageCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_session, (object)null));
            }

            [Test]
            public void ProjectAsyncToken_ClientCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(null, new object(), CancellationToken.None));
            }

            [Test]
            public void ProjectAsyncToken_MessageCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(
                    async () => await sut.ProjectAsync(_session, (object)null, CancellationToken.None));
            }

            [Test]
            public void ProjectManyAsync_ClientCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(null, new object[0]));
            }

            [Test]
            public void ProjectManyAsync_MessageCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_session, (object[])null));
            }

            [Test]
            public void ProjectManyAsyncToken_ClientCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(null, new object[0], CancellationToken.None));
            }

            [Test]
            public void ProjectManyAsyncToken_MessageCanNotBeNull()
            {
                var sut = SutFactory();
                Assert.Throws<ArgumentNullException>(
                    async () => await sut.ProjectAsync(_session, (object[])null, CancellationToken.None));
            }

            private static AsyncRavenProjector SutFactory()
            {
                return new AsyncRavenProjector(new RavenProjectionHandler[0]);
            }
        }

        [TestFixture]
        public class SingleHandlerTests
        {
            private List<RavenProjectionHandlerCall> _calls;
            private AsyncRavenProjector _sut;

            private EmbeddableDocumentStore _store;
            private IAsyncDocumentSession _session;

            [SetUp]
            public void SetUp()
            {
                _store = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    DataDirectory = Path.GetTempPath()
                };
                _store.Initialize();
                _session = _store.OpenAsyncSession();
                _calls = new List<RavenProjectionHandlerCall>();
                var handler = new RavenProjectionHandler(
                    typeof(MatchMessage1),
                    (session, msg, token) =>
                    {
                        _calls.Add(new RavenProjectionHandlerCall(session, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] { handler });
            }

            [TearDown]
            public void TearDown()
            {
                _session.Dispose();
                _store.Dispose();
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_session, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_session, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message, source.Token)
                }));
            }

            [Test]
            public async void ProjectManyAsyncHasExpectedResult()
            {
                var message1 = new MatchMessage1();
                var message2 = new MatchMessage1();
                var messages = new object[]
                {
                    message1,
                    new MismatchMessage(),
                    message2
                };
                await _sut.ProjectAsync(_session, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message1, CancellationToken.None),
                    new RavenProjectionHandlerCall(_session, message2, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectManyAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message1 = new MatchMessage1();
                var message2 = new MatchMessage1();
                var messages = new object[]
                {
                    message1,
                    new MismatchMessage(),
                    message2,
                };

                await _sut.ProjectAsync(_session, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message1, source.Token),
                    new RavenProjectionHandlerCall(_session, message2, source.Token),
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_session, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_session, message, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectManyAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var messages = new object[]
                {
                    new MismatchMessage(),
                    new MismatchMessage()
                };
                await _sut.ProjectAsync(_session, messages);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectManyAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var messages = new object[]
                {
                    new MismatchMessage(),
                    new MismatchMessage()
                };
                await _sut.ProjectAsync(_session, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncRavenProjector SutFactory(RavenProjectionHandler[] handlers)
            {
                return new AsyncRavenProjector(handlers);
            }
        }

        [TestFixture]
        public class MultiHandlerTests
        {
            private List<RavenProjectionHandlerCall> _calls;
            private AsyncRavenProjector _sut;

            private EmbeddableDocumentStore _store;
            private IAsyncDocumentSession _session;

            [SetUp]
            public void SetUp()
            {
                _store = new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    DataDirectory = Path.GetTempPath()
                };
                _store.Initialize();
                _session = _store.OpenAsyncSession();
                _calls = new List<RavenProjectionHandlerCall>();
                var handler1 = new RavenProjectionHandler(
                    typeof(MatchMessage1),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RavenProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler2 = new RavenProjectionHandler(
                    typeof(MatchMessage2),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RavenProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler3 = new RavenProjectionHandler(
                    typeof(MatchMessage1),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RavenProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler4 = new RavenProjectionHandler(
                    typeof(MatchMessage2),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RavenProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] { handler1, handler2, handler3, handler4 });
            }

            [TearDown]
            public void TearDown()
            {
                _session.Dispose();
                _store.Dispose();
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_session, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message, CancellationToken.None),
                    new RavenProjectionHandlerCall(_session, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_session, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message, source.Token),
                    new RavenProjectionHandlerCall(_session, message, source.Token)
                }));
            }

            [Test]
            public async void ProjectManyAsyncHasExpectedResult()
            {
                var message1 = new MatchMessage1();
                var message2 = new MatchMessage2();
                var messages = new object[]
                {
                    message1,
                    new MismatchMessage(),
                    message2
                };
                await _sut.ProjectAsync(_session, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message1, CancellationToken.None),
                    new RavenProjectionHandlerCall(_session, message1, CancellationToken.None),
                    new RavenProjectionHandlerCall(_session, message2, CancellationToken.None),
                    new RavenProjectionHandlerCall(_session, message2, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectManyAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message1 = new MatchMessage1();
                var message2 = new MatchMessage2();
                var messages = new object[]
                {
                    message1,
                    new MismatchMessage(),
                    message2,
                };

                await _sut.ProjectAsync(_session, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RavenProjectionHandlerCall(_session, message1, source.Token),
                    new RavenProjectionHandlerCall(_session, message1, source.Token),
                    new RavenProjectionHandlerCall(_session, message2, source.Token),
                    new RavenProjectionHandlerCall(_session, message2, source.Token)
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_session, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_session, message, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectManyAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var messages = new object[]
                {
                    new MismatchMessage(),
                    new MismatchMessage()
                };
                await _sut.ProjectAsync(_session, messages);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectManyAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var messages = new object[]
                {
                    new MismatchMessage(),
                    new MismatchMessage()
                };
                await _sut.ProjectAsync(_session, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncRavenProjector SutFactory(RavenProjectionHandler[] handlers)
            {
                return new AsyncRavenProjector(handlers);
            }
        }

        public class RavenProjectionHandlerCall
        {
            public readonly IAsyncDocumentSession Client;
            public readonly object Message;
            public readonly CancellationToken Token;

            public RavenProjectionHandlerCall(IAsyncDocumentSession connection, object message, CancellationToken token)
            {
                Client = connection;
                Message = message;
                Token = token;
            }

            private bool Equals(RavenProjectionHandlerCall other)
            {
                return Equals(Client, other.Client) &&
                       Equals(Message, other.Message) &&
                       Token.Equals(other.Token);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((RavenProjectionHandlerCall)obj);
            }

            public override int GetHashCode()
            {
                var hashCode = (Client != null ? Client.GetHashCode() : 0);
                hashCode ^= (Message != null ? Message.GetHashCode() : 0);
                hashCode ^= Token.GetHashCode();
                return hashCode;
            }
        }

        public class MatchMessage1
        {
        }

        public class MatchMessage2
        {
        }

        public class MismatchMessage
        {
        }
    }
}
