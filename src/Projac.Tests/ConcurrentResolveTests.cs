using System;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class ConcurrentResolveTests
    {
        [Test]
        public void WhenHandlerMessageTypeHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                ConcurrentResolve.WhenHandlerMessageType(null));
        }

        [Test]
        public void WhenHandlerMessageTypeResolverThrowsWhenMessageIsNull()
        {
            var sut = ConcurrentResolve.WhenHandlerMessageType(new SqlProjectionHandler[0]);
            Assert.Throws<ArgumentNullException>(() => sut(null));
        }

        [TestCaseSource(typeof(HandlerResolutionCases), "WhenHandlerMessageTypeCases")]
        public void WhenHandlerMessageTypeResolverReturnsExpectedResult(
            SqlProjectionHandler[] resolvable,
            object message,
            SqlProjectionHandler[] resolved)
        {
            var sut = ConcurrentResolve.WhenHandlerMessageType(resolvable);
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