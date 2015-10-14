using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    [TestFixture]
    public class ConnectedProjectionHandlerTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedProjectionHandler<object>(null, (_, __, ___) => Task.FromResult<object>(null))
                );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ConnectedProjectionHandler<object>(typeof(object), null)
                );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<object, object, CancellationToken, Task> handler = (_, __, ___) => Task.FromResult<object>(null);

            var sut = new ConnectedProjectionHandler<object>(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}