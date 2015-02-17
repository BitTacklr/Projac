using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

namespace Projac.WindowsAzure.Storage.Tests
{
    namespace AsyncCloudTableProjectorTests
    {
        [TestFixture]
        public class GuardTests
        {
            private CloudTableClient _client;

            [SetUp]
            public void SetUp()
            {
                var account = CloudStorageAccount.DevelopmentStorageAccount;
                _client = account.CreateCloudTableClient();
            }

            [Test]
            public void HandlersCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new AsyncCloudTableProjector(null));
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
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_client, (object) null));
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
                    async () => await sut.ProjectAsync(_client, (object) null, CancellationToken.None));
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
                Assert.Throws<ArgumentNullException>(async () => await sut.ProjectAsync(_client, (object[]) null));
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
                    async () => await sut.ProjectAsync(_client, (object[]) null, CancellationToken.None));
            }

            private static AsyncCloudTableProjector SutFactory()
            {
                return new AsyncCloudTableProjector(new CloudTableProjectionHandler[0]);
            }
        }

        [TestFixture]
        public class SingleHandlerTests
        {
            private CloudTableClient _client;
            private List<CloudTableProjectionHandlerCall> _calls;
            private AsyncCloudTableProjector _sut;

            [SetUp]
            public void SetUp()
            {
                var account = CloudStorageAccount.DevelopmentStorageAccount;
                _client = account.CreateCloudTableClient();
                _calls = new List<CloudTableProjectionHandlerCall>();
                var handler = new CloudTableProjectionHandler(
                    typeof(MatchMessage1),
                    (client, msg, token) =>
                    {
                        _calls.Add(new CloudTableProjectionHandlerCall(client, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] { handler });
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_client, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_client, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message, source.Token)
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
                await _sut.ProjectAsync(_client, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message1, CancellationToken.None),
                    new CloudTableProjectionHandlerCall(_client, message2, CancellationToken.None)
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
                
                await _sut.ProjectAsync(_client, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message1, source.Token),
                    new CloudTableProjectionHandlerCall(_client, message2, source.Token),
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_client, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_client, message, source.Token);
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
                await _sut.ProjectAsync(_client, messages);
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
                await _sut.ProjectAsync(_client, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncCloudTableProjector SutFactory(CloudTableProjectionHandler[] handlers)
            {
                return new AsyncCloudTableProjector(handlers);
            }
        }

        [TestFixture]
        public class MultiHandlerTests
        {
            private CloudTableClient _client;
            private List<CloudTableProjectionHandlerCall> _calls;
            private AsyncCloudTableProjector _sut;

            [SetUp]
            public void SetUp()
            {
                var account = CloudStorageAccount.DevelopmentStorageAccount;
                _client = account.CreateCloudTableClient();
                _calls = new List<CloudTableProjectionHandlerCall>();
                var handler1 = new CloudTableProjectionHandler(
                    typeof (MatchMessage1),
                    (client, msg, token) =>
                    {
                        _calls.Add(new CloudTableProjectionHandlerCall(client, msg, token));
                        return Task.FromResult(false);
                    });
                var handler2 = new CloudTableProjectionHandler(
                    typeof(MatchMessage2),
                    (client, msg, token) =>
                    {
                        _calls.Add(new CloudTableProjectionHandlerCall(client, msg, token));
                        return Task.FromResult(false);
                    });
                var handler3 = new CloudTableProjectionHandler(
                    typeof(MatchMessage1),
                    (client, msg, token) =>
                    {
                        _calls.Add(new CloudTableProjectionHandlerCall(client, msg, token));
                        return Task.FromResult(false);
                    });
                var handler4 = new CloudTableProjectionHandler(
                    typeof(MatchMessage2),
                    (client, msg, token) =>
                    {
                        _calls.Add(new CloudTableProjectionHandlerCall(client, msg, token));
                        return Task.FromResult(false);
                    });
                _sut = SutFactory(new[] {handler1, handler2, handler3, handler4});
            }

            [Test]
            public async void ProjectAsyncHasExpectedResult()
            {
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_client, message);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message, CancellationToken.None),
                    new CloudTableProjectionHandlerCall(_client, message, CancellationToken.None)
                }));
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResult()
            {
                var source = new CancellationTokenSource();
                var message = new MatchMessage1();
                await _sut.ProjectAsync(_client, message, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message, source.Token),
                    new CloudTableProjectionHandlerCall(_client, message, source.Token)
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
                await _sut.ProjectAsync(_client, messages);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message1, CancellationToken.None),
                    new CloudTableProjectionHandlerCall(_client, message1, CancellationToken.None),
                    new CloudTableProjectionHandlerCall(_client, message2, CancellationToken.None),
                    new CloudTableProjectionHandlerCall(_client, message2, CancellationToken.None)
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

                await _sut.ProjectAsync(_client, messages, source.Token);
                Assert.That(_calls, Is.EquivalentTo(new[]
                {
                    new CloudTableProjectionHandlerCall(_client, message1, source.Token),
                    new CloudTableProjectionHandlerCall(_client, message1, source.Token),
                    new CloudTableProjectionHandlerCall(_client, message2, source.Token),
                    new CloudTableProjectionHandlerCall(_client, message2, source.Token)
                }));
            }

            [Test]
            public async void ProjectAsyncHasExpectedResultWhenMessageTypeMismatch()
            {
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_client, message);
                Assert.That(_calls, Is.Empty);
            }

            [Test]
            public async void ProjectAsyncTokenHasExpectedResultWhenMessageTypeMismatch()
            {
                var source = new CancellationTokenSource();
                var message = new MismatchMessage();
                await _sut.ProjectAsync(_client, message, source.Token);
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
                await _sut.ProjectAsync(_client, messages);
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
                await _sut.ProjectAsync(_client, messages, source.Token);
                Assert.That(_calls, Is.Empty);
            }

            private static AsyncCloudTableProjector SutFactory(CloudTableProjectionHandler[] handlers)
            {
                return new AsyncCloudTableProjector(handlers);
            }
        }

        public class CloudTableProjectionHandlerCall
        {
            public readonly CloudTableClient Client;
            public readonly object Message;
            public readonly CancellationToken Token;

            public CloudTableProjectionHandlerCall(CloudTableClient client, object message, CancellationToken token)
            {
                Client = client;
                Message = message;
                Token = token;
            }

            private bool Equals(CloudTableProjectionHandlerCall other)
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
                return Equals((CloudTableProjectionHandlerCall)obj);
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