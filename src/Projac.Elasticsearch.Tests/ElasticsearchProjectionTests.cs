using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Elasticsearch.Tests
{
    [TestFixture]
    public class ElasticsearchProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new ElasticsearchProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new ElasticsearchProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = ElasticsearchProjection.Empty;

            Assert.That(result, Is.InstanceOf<ElasticsearchProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(ElasticsearchProjection.Empty, ElasticsearchProjection.Empty);
        }

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => ElasticsearchProjection.Empty.Concat((ElasticsearchProjection)null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => ElasticsearchProjection.Empty.Concat((ElasticsearchProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => ElasticsearchProjection.Empty.Concat((ElasticsearchProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler4 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var projection = new ElasticsearchProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new ElasticsearchProjection(new[]
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
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new ElasticsearchProjection(new[]
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
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler4 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new ElasticsearchProjection(new[]
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
            var sut = ElasticsearchProjection.Empty;

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new ElasticsearchProjection(new[]
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
        public void ImplicitConversionToElasticsearchProjectionHandlerArray()
        {
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new ElasticsearchProjection(handlers);

            ElasticsearchProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToElasticsearchProjectionHandlerArray()
        {
            var handler1 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new ElasticsearchProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new ElasticsearchProjection(handlers);

            var result = (ElasticsearchProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}