using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StackExchange.Redis;

namespace Projac.Redis.Tests
{
    namespace AsyncRedisProjectorTests
    {
        [TestFixture]
        public class GuardTests
        {
            private ConnectionMultiplexer _connection;

            [SetUp]
            public void SetUp()
            {
                _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    ConnectTimeout = 1,
                    EndPoints =
                    {
                        { IPAddress.Loopback, 1234 }
                    }
                });
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AsyncRedisProjector(null));
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
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_connection, (object) null));
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
                    async () => await sut.ProjectAsync(_connection, (object) null, CancellationToken.None));
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
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_connection, (object[]) null));
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
                    async () => await sut.ProjectAsync(_connection, (object[]) null, CancellationToken.None));
            }

            private static AsyncRedisProjector SutFactory()
            {
                return new AsyncRedisProjector(new RedisProjectionHandler[0]);
            }
        }

        [TestFixture]
        public class SingleHandlerTests
        {
            private ConnectionMultiplexer _connection;
            private List<RedisProjectionHandlerCall> _calls;
            private AsyncRedisProjector _sut;

            [SetUp]
            public void SetUp()
            {
                _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    ConnectTimeout = 1,
                    EndPoints =
                    {
                        { IPAddress.Loopback, 1234 }
                    }
                });
                _calls = new List<RedisProjectionHandlerCall>();
                var handler = new RedisProjectionHandler(
                    typeof(MatchMessage1),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RedisProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] { handler });
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_connection, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_connection, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message, source.Token)
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
                await _sut.ProjectAsync(_connection, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message1, CancellationToken.None),
                    new RedisProjectionHandlerCall(_connection, message2, CancellationToken.None)
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

                await _sut.ProjectAsync(_connection, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message1, source.Token),
                    new RedisProjectionHandlerCall(_connection, message2, source.Token),
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_connection, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_connection, message, source.Token);
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
                await _sut.ProjectAsync(_connection, messages);
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
                await _sut.ProjectAsync(_connection, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncRedisProjector SutFactory(RedisProjectionHandler[] handlers)
            {
                return new AsyncRedisProjector(handlers);
            }
        }

        [TestFixture]
        public class MultiHandlerTests
        {
            private ConnectionMultiplexer _connection;
            private List<RedisProjectionHandlerCall> _calls;
            private AsyncRedisProjector _sut;

            [SetUp]
            public void SetUp()
            {
                _connection = ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    ConnectTimeout = 1,
                    EndPoints =
                    {
                        { IPAddress.Loopback, 1234 }
                    }
                });
                _calls = new List<RedisProjectionHandlerCall>();
                var handler1 = new RedisProjectionHandler(
                    typeof(MatchMessage1),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RedisProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler2 = new RedisProjectionHandler(
                    typeof(MatchMessage2),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RedisProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler3 = new RedisProjectionHandler(
                    typeof(MatchMessage1),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RedisProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                var handler4 = new RedisProjectionHandler(
                    typeof(MatchMessage2),
                    (connection, msg, token) =>
                    {
                        _calls.Add(new RedisProjectionHandlerCall(connection, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] { handler1, handler2, handler3, handler4 });
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_connection, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message, CancellationToken.None),
                    new RedisProjectionHandlerCall(_connection, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_connection, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message, source.Token),
                    new RedisProjectionHandlerCall(_connection, message, source.Token)
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
                await _sut.ProjectAsync(_connection, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message1, CancellationToken.None),
                    new RedisProjectionHandlerCall(_connection, message1, CancellationToken.None),
                    new RedisProjectionHandlerCall(_connection, message2, CancellationToken.None),
                    new RedisProjectionHandlerCall(_connection, message2, CancellationToken.None)
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

                await _sut.ProjectAsync(_connection, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new RedisProjectionHandlerCall(_connection, message1, source.Token),
                    new RedisProjectionHandlerCall(_connection, message1, source.Token),
                    new RedisProjectionHandlerCall(_connection, message2, source.Token),
                    new RedisProjectionHandlerCall(_connection, message2, source.Token)
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_connection, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_connection, message, source.Token);
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
                await _sut.ProjectAsync(_connection, messages);
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
                await _sut.ProjectAsync(_connection, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncRedisProjector SutFactory(RedisProjectionHandler[] handlers)
            {
                return new AsyncRedisProjector(handlers);
            }
        }

        public class RedisProjectionHandlerCall
        {
            public readonly ConnectionMultiplexer Client;
            public readonly object Message;
            public readonly CancellationToken Token;

            public RedisProjectionHandlerCall(ConnectionMultiplexer connection, object message, CancellationToken token)
            {
                Client = connection;
                Message = message;
                Token = token;
            }

            private bool Equals(RedisProjectionHandlerCall other)
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
                return Equals((RedisProjectionHandlerCall)obj);
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