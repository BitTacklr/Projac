using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionTests
    {
        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = TSqlProjection.Empty;
            Assert.That(result.Handlers, Is.EquivalentTo(new TSqlProjectionHandler[0]));
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            var instance1 = TSqlProjection.Empty;
            var instance2 = TSqlProjection.Empty;
            Assert.That(instance1, Is.SameAs(instance2));
        }

        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TSqlProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new TSqlProjectionHandler(typeof(object), _ => new TSqlNonQueryStatement[0]);
            var handler2 = new TSqlProjectionHandler(typeof(object), _ => new TSqlNonQueryStatement[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new TSqlProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.InstanceOf<IReadOnlyCollection<TSqlProjectionHandler>>());
            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}