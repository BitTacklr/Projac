using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.WindowsAzure.Storage.Tests
{
    [TestFixture]
    public class CloudTableProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CloudTableProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new CloudTableProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = CloudTableProjection.Empty;

            Assert.That(result, Is.InstanceOf<CloudTableProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(CloudTableProjection.Empty, CloudTableProjection.Empty);
        }

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => CloudTableProjection.Empty.Concat((CloudTableProjection)null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => CloudTableProjection.Empty.Concat((CloudTableProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => CloudTableProjection.Empty.Concat((CloudTableProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler4 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var projection = new CloudTableProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new CloudTableProjection(new[]
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
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new CloudTableProjection(new[]
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
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler3 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler4 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new CloudTableProjection(new[]
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
            var sut = CloudTableProjection.Empty;

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var sut = new CloudTableProjection(new[]
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
        public void ImplicitConversionToCloudTableProjectionHandlerArray()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new CloudTableProjection(handlers);

            CloudTableProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToCloudTableProjectionHandlerArray()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new CloudTableProjection(handlers);

            var result = (CloudTableProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}