using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Redis.Tests
{
    [TestFixture]
    public class RedisProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RedisProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RedisProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = RedisProjection.Empty;

            Assert.That(result, Is.InstanceOf<RedisProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(RedisProjection.Empty, RedisProjection.Empty);
        }

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RedisProjection.Empty.Concat((RedisProjection)null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RedisProjection.Empty.Concat((RedisProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RedisProjection.Empty.Concat((RedisProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler4 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var projection = new RedisProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new RedisProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.Concat(projection);

            Assert.That(result.Handlers, Is.EquivalentTo(new[] { handler1, handler2, handler3, handler4 }));
        }

        [Test]
        public void ConcatHandlerReturnsExpectedResult()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RedisProjection(new[]
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
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler4 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RedisProjection(new[]
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
        public void EmptyToBuilderReturnsExpectedResult()
        {
            var sut = RedisProjection.Empty;

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RedisProjection(new[]
            {
                handler1,
                handler2
            });

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.EquivalentTo(new[]
            {
                handler1,
                handler2
            }));
        }

        [Test]
        public void ImplicitConversionToRedisProjectionHandlerArray()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RedisProjection(handlers);

            RedisProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToRedisProjectionHandlerArray()
        {
            var handler1 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RedisProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RedisProjection(handlers);

            var result = (RedisProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}