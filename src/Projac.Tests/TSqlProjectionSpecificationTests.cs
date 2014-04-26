using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionSpecificationTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TSqlProjectionSpecification(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new TSqlProjectionHandler(typeof(object), _ => new ITSqlStatement[0]);
            var handler2 = new TSqlProjectionHandler(typeof(object), _ => new ITSqlStatement[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new TSqlProjectionSpecification(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.InstanceOf<IReadOnlyCollection<TSqlProjectionHandler>>());
            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}