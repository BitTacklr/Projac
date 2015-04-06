using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Raven.Client;

namespace Projac.RavenDB.Tests
{
    [TestFixture]
    public class RavenProjectionHandlerTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RavenProjectionHandler(null, (session, message, token) => Task.FromResult(false))
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RavenProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<IAsyncDocumentSession, object, CancellationToken, Task> handler = (session, msg, token) => Task.FromResult(false);

            var sut = new RavenProjectionHandler(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
