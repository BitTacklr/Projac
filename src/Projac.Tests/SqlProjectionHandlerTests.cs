using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionHandlerTests
    {
        [Test]
        public void EventCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SqlProjectionHandler(null, _ => new SqlNonQueryStatement[0])
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SqlProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var @event = typeof(object);
            Func<object, IEnumerable<SqlNonQueryStatement>> handler = _ => new SqlNonQueryStatement[0];

            var sut = new SqlProjectionHandler(@event, handler);

            Assert.That(sut.Event, Is.EqualTo(@event));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
