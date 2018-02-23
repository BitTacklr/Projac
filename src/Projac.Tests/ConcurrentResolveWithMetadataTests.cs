using System;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class ConcurrentResolveWithMetadataTests
    {
        [Test]
        public void WhenEqualToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenEqualToHandlerMessageType<object, object>(null));
        }

        [Test]
        public void WhenEqualToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(new ProjectionHandler<object, object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerWithMetadataResolutionCases), nameof(HandlerWithMetadataResolutionCases.WhenEqualToHandlerMessageTypeCases))]
        public void WhenEqualToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object, object>[] resolvable,
            object message,
            ProjectionHandler<object, object>[] resolved)
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenAssignableToHandlerMessageType<object, object>(null));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(new ProjectionHandler<object, object>[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerWithMetadataResolutionCases), nameof(HandlerWithMetadataResolutionCases.WhenAssignableToHandlerMessageTypeCases))]
        public void WhenAssignableToHandlerMessageTypeResolverReturnsExpectedResult(
            ProjectionHandler<object, object>[] resolvable,
            object message,
            ProjectionHandler<object, object>[] resolved)
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }
    }
}