using System;
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

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = SqlProjection.Empty;

            Assert.That(result, Is.InstanceOf<SqlProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(SqlProjection.Empty, SqlProjection.Empty);
        }
    }
}