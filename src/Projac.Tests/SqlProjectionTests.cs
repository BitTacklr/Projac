using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SqlProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryStatement[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryStatement[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.InstanceOf<IReadOnlyCollection<SqlProjectionHandler>>());
            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}