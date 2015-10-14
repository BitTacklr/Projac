using System;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    [TestFixture]
    public class ResolveTests
    {
        [Test]
        public void WhenEqualToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Resolve.WhenEqualToHandlerMessageType<object>(null));
        }

        [Test]
        public void WhenEqualToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = Resolve.WhenEqualToHandlerMessageType(new ConnectedProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenEqualToHandlerMessageTypeCases")]
        public void WhenEqualToHandlerMessageTypeResolverReturnsExpectedResult(
            ConnectedProjectionHandler<object>[] resolvable,
            object message,
            ConnectedProjectionHandler<object>[] resolved)
        {
            var sut = Resolve.WhenEqualToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                Resolve.WhenAssignableToHandlerMessageType<object>(null));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = Resolve.WhenAssignableToHandlerMessageType(new ConnectedProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenAssignableToHandlerMessageTypeCases")]
        public void WhenAssignableToHandlerMessageTypeResolverReturnsExpectedResult(
            ConnectedProjectionHandler<object>[] resolvable,
            object message,
            ConnectedProjectionHandler<object>[] resolved)
        {
            var sut = Resolve.WhenAssignableToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }
    }
}
