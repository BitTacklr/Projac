using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    namespace ProjectionScenarioTests
    {
        [TestFixture]
        public class AnyState
        {
            [Test]
            public void ResolverCanNotBeNull()
            {
                Assert.Throws<ArgumentNullException>(
                    () => new ProjectionScenario<object>(null));
            }

            [Test]
            public void GivenArrayCanNotBeNull()
            {
                var sut = SutFactory.Create<object>();
                Assert.Throws<ArgumentNullException>(
                    () => sut.Given((object[])null));
            }

            [Test]
            public void GivenEnumerableCanNotBeNull()
            {
                var sut = SutFactory.Create<object>();
                Assert.Throws<ArgumentNullException>(
                    () => sut.Given((IEnumerable<object>)null));
            }

            [Test]
            public void VerifyWithTokenVerificationCanNotBeNull()
            {
                var sut = SutFactory.Create<object>();
                Assert.Throws<ArgumentNullException>(
                    () => sut.Verify((Func<object, CancellationToken, Task<VerificationResult>>) null));
            }

            [Test]
            public void VerifyWithoutTokenVerificationCanNotBeNull()
            {
                var sut = SutFactory.Create<object>();
                Assert.Throws<ArgumentNullException>(
                    () => sut.Verify((Func<object, Task<VerificationResult>>)null));
            }

            [Test]
            public void VerifyWithoutTokenReturnsExpectedResult()
            {
                var sut = SutFactory.Create<object>();
                var session = new object();
                var result = sut.
                    Verify(connection =>
                    {
                        Assert.That(connection, Is.EqualTo(session));
                        return Task.FromResult(VerificationResult.Pass());
                    }).
                    Verification(session, CancellationToken.None).
                    Result;

                Assert.That(result, Is.EqualTo(VerificationResult.Pass()));
            }

            [Test]
            public void VerifyWithTokenReturnsExpectedResult()
            {
                var sut = SutFactory.Create<object>();
                var session = new object();
                var sessionToken = new CancellationToken();
                var result = sut.
                    Verify((connection, token) =>
                    {
                        Assert.That(connection, Is.EqualTo(session));
                        Assert.That(token, Is.EqualTo(sessionToken));
                        return Task.FromResult(VerificationResult.Pass());
                    }).
                    Verification(session, sessionToken).
                    Result;

                Assert.That(result, Is.EqualTo(VerificationResult.Pass()));
            }

            [Test]
            public void ResolverReturnsExpectedResultAfterVerifyWithToken()
            {
                var resolver = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
                var sut = SutFactory.Create(resolver);

                var result = sut.Verify((session, token) => null).Resolver;

                Assert.That(result, Is.EqualTo(resolver));
            }

            [Test]
            public void ResolverReturnsExpectedResultAfterVerifyWithoutToken()
            {
                var resolver = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
                var sut = SutFactory.Create(resolver);

                var result = sut.Verify(session => null).Resolver;

                Assert.That(result, Is.EqualTo(resolver));
            }
        }

        [TestFixture]
        public class InitialState
        {
            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };
                var sut = SutFactory.Create<object>();

                var result = sut.Given(messages).Verify((session, token) => null).Messages;

                Assert.That(result, Is.EqualTo(messages));
            }

            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithoutToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };
                var sut = SutFactory.Create<object>();

                var result = sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(messages));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithToken()
            {
                IEnumerable<object> messages = new []
                {
                    new object(),
                    new object()
                };
                var sut = SutFactory.Create<object>();

                var result = sut.Given(messages).Verify((session, token) => null).Messages;
                
                Assert.That(result, Is.EqualTo(messages));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithoutToken()
            {
                IEnumerable<object> messages = new[]
                {
                    new object(),
                    new object()
                };
                var sut = SutFactory.Create<object>();

                var result = sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(messages));
            }
        }

        [TestFixture]
        public class GivenArrayState
        {
            private ProjectionHandlerResolver<object> _resolver;
            private ProjectionScenario<object> _sut;
            private object[] _messages;

            [SetUp]
            public void SetUp()
            {
                _resolver = Resolve.
                    WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
                _messages = new[] { new object(), new object() };
                _sut = new ProjectionScenario<object>(_resolver).Given(_messages);
            }

            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify((session, token) => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithoutToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithToken()
            {
                IEnumerable<object> messages = new[]
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify((session, token) => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithoutToken()
            {
                IEnumerable<object> messages = new[]
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }
        }

        [TestFixture]
        public class GivenEnumerableState
        {
            private ProjectionHandlerResolver<object> _resolver;
            private ProjectionScenario<object> _sut;
            private IEnumerable<object> _messages;

            [SetUp]
            public void SetUp()
            {
                _resolver = Resolve.
                    WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
                _messages = new[] { new object(), new object() };
                _sut = new ProjectionScenario<object>(_resolver).Given(_messages);
            }

            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify((session, token) => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenArrayReturnsExpectedResultAfterVerifyWithoutToken()
            {
                object[] messages =
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithToken()
            {
                IEnumerable<object> messages = new[]
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify((session, token) => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }

            [Test]
            public void GivenEnumerableReturnsExpectedResultAfterVerifyWithoutToken()
            {
                IEnumerable<object> messages = new[]
                {
                    new object(),
                    new object()
                };

                var result = _sut.Given(messages).Verify(session => null).Messages;

                Assert.That(result, Is.EqualTo(_messages.Concat(messages).ToArray()));
            }
        }

        internal static class SutFactory
        {
            public static ProjectionScenario<TConnection> Create<TConnection>()
            {
                return new ProjectionScenario<TConnection>(
                    Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<TConnection>[0]));
            }

            public static ProjectionScenario<TConnection> Create<TConnection>(ProjectionHandlerResolver<TConnection> resolver)
            {
                return new ProjectionScenario<TConnection>(resolver);
            }
        }
    }
}
