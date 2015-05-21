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
                ConcurrentResolve.WhenEqualToHandlerMessageType(null));
        }

        [Test]
        public void WhenEqualToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(new SqlProjectionHandler[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenEqualToHandlerMessageTypeCases")]
        public void WhenEqualToHandlerMessageTypeResolverReturnsExpectedResult(
            SqlProjectionHandler[] resolvable,
            object message,
            SqlProjectionHandler[] resolved)
        {
            var sut = ConcurrentResolve.WhenEqualToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenAssignableToHandlerMessageType(null));
        }

        [Test]
        public void WhenAssignableToHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(new SqlProjectionHandler[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenAssignableToHandlerMessageTypeCases")]
        public void WhenAssignableToHandlerMessageTypeResolverReturnsExpectedResult(
            SqlProjectionHandler[] resolvable,
            object message,
            SqlProjectionHandler[] resolved)
        {
            var sut = ConcurrentResolve.WhenAssignableToHandlerMessageType(resolvable);
            var result = sut(message);
            Assert.That(result, Is.EquivalentTo(resolved));
        }
    }
}