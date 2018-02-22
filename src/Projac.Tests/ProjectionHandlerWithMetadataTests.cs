using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class ProjectionHandlerWithMetadataTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProjectionHandler<object, object>(null, (_, __, ___, ____) => Task.CompletedTask)
                );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ProjectionHandler<object>(typeof(object), null)
                );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<object, object, object, CancellationToken, Task> handler = (_, __, ___, ____) => Task.CompletedTask;

            var sut = new ProjectionHandler<object, object>(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}