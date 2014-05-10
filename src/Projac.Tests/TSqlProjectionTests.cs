using System;
using System.Collections.Generic;
using System.Data.Odbc;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionTests
    {
        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            Assert.That(
                TSqlProjection.Empty,
                Is.EqualTo(new TSqlProjection(new TSqlProjectionHandler[0])).
                    Using(new EmptyTSqlProjectionEqualityComparer()));
        }

        class EmptyTSqlProjectionEqualityComparer : IEqualityComparer<TSqlProjection>
        {
            public bool Equals(TSqlProjection x, TSqlProjection y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (x == null || y == null) return false;
                return x.Handlers.Count == 0 && y.Handlers.Count == 0;
            }

            public int GetHashCode(TSqlProjection obj)
            {
                throw new NotSupportedException();
            }
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