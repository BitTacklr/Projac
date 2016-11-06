using System;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class ConcurrentResolveTests
    {
        [Test]
        public void WhenEqualToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenEqualToHandlerMessageType<object>(null));
        }

        [Test]
        public void WhenEqualToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenEqualToHandlerMessageTypeCases")]
        public void WhenEqualToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object>[] resolvable,
            object message,
            ProjectionHandler<object>[] resolved)
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenAssignableToHandlerMessageType<object>(null));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(new ProjectionHandler<object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenAssignableToHandlerMessageTypeCases")]
        public void WhenAssignableToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object>[] resolvable,
            object message,
            ProjectionHandler<object>[] resolved)
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }
    }
}