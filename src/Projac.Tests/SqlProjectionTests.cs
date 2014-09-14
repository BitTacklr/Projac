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
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

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

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjection) null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SqlProjection.Empty.Concat((SqlProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler4 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var projection = new SqlProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(projection);

            Assert.That(result.Handlers, Is.EquivalentTo(new[]{ handler1, handler2, handler3, handler4}));
        }

        [Test]
        public void ConcatHandlerReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(handler3);

            Assert.That(result.Handlers, Is.EquivalentTo(new[] { handler1, handler2, handler3 }));
        }

        [Test]
        public void ConcatHandlersReturnsExpectedResult()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler3 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler4 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var sut = new SqlProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(new[]
            {
                handler3,
                handler4
            });

            Assert.That(result.Handlers, Is.EquivalentTo(new[] { handler1, handler2, handler3, handler4 }));
        }

        [Test]
        public void ImplicitConversionToSqlProjectionHandlerArray()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            SqlProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToSqlProjectionHandlerArray()
        {
            var handler1 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);
            var handler2 = new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]);

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new SqlProjection(handlers);

            var result = (SqlProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}