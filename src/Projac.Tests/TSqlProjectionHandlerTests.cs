using System;
using System.Collections.Generic;
using NUnit.Framework;
using TSqlClient;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionHandlerTests
    {
        [Test]
        public void EventCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TSqlProjectionHandler(null, _ => new TSqlNonQueryStatement[0])
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TSqlProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var @event = typeof(object);
            Func<object, IEnumerable<TSqlNonQueryStatement>> handler = _ => new TSqlNonQueryStatement[0];

            var sut = new TSqlProjectionHandler(@event, handler);

            Assert.That(sut.Event, Is.EqualTo(@event));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
