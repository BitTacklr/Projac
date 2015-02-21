using System;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using NUnit.Framework;

namespace Projac.Elasticsearch.Tests
{
    [TestFixture]
    public class ElasticsearchProjectionHandlerTests
    {
        [Test]
        public void MessageCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ElasticsearchProjectionHandler(null, (client, message, token) => Task.FromResult(false))
            );
        }

        [Test]
        public void HandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ElasticsearchProjectionHandler(typeof(object), null)
            );
        }

        [Test]
        public void ParametersArePreservedAsProperties()
        {
            var message = typeof(object);
            Func<IElasticsearchClient, object, CancellationToken, Task> handler = (client, msg, token) => Task.FromResult(false);

            var sut = new ElasticsearchProjectionHandler(message, handler);

            Assert.That(sut.Message, Is.EqualTo(message));
            Assert.That(sut.Handler, Is.EqualTo(handler));
        }
    }
}
