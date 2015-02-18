using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using StackExchange.Redis;

namespace Projac.Redis.Tests
{
    [TestFixture]
    public class RedisProjectionHandlerTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RedisProjectionHandler(null, (connection, message, token) => Task.FromResult(false))
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RedisProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<ConnectionMultiplexer, object, CancellationToken, Task> handler = (connection, msg, token) => Task.FromResult(false);

            var sut = new RedisProjectionHandler(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
