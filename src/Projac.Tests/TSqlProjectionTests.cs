using System;
using System.Collections.Generic;
using NUnit.Framework;
using TSqlClient;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionTests
    {
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