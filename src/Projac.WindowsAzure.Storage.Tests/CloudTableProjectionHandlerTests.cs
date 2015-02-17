using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

namespace Projac.WindowsAzure.Storage.Tests
{
    [TestFixture]
    public class CloudTableProjectionHandlerTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CloudTableProjectionHandler(null, (client, message, token) => Task.FromResult(false))
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CloudTableProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<CloudTableClient, object, CancellationToken, Task> handler = (client, msg, token) => Task.FromResult(false);

            var sut = new CloudTableProjectionHandler(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
