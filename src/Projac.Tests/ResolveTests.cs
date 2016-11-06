using System;
using NUnit.Framework;

namespace Projac.Tests
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
            var sut = Resolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenEqualToHandlerMessageTypeCases")]
        public void WhenEqualToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object>[] resolvable,
            object message,
            ProjectionHandler<object>[] resolved)
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
            var sut = Resolve.WhenAssignableToHandlerMessageType(new ProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenAssignableToHandlerMessageTypeCases")]
        public void WhenAssignableToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object>[] resolvable,
            object message,
            ProjectionHandler<object>[] resolved)
        {
            var sut = Resolve.WhenAssignableToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }
    }
}
