using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.RavenDB.Tests
{
    [TestFixture]
    public class RavenProjectionTests
    {
        [Test]
        public void HandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => new RavenProjection(null)
                );
        }

        [Test]
        public void HandlersArePreservedAsProperty()
        {
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RavenProjection(handlers);

            var result = sut.Handlers;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void EmptyReturnsExpectedInstance()
        {
            var result = RavenProjection.Empty;

            Assert.That(result, Is.InstanceOf<RavenProjection>());
            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void EmptyReturnsSameInstance()
        {
            Assert.AreSame(RavenProjection.Empty, RavenProjection.Empty);
        }

        [Test]
        public void ConcatProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RavenProjection.Empty.Concat((RavenProjection)null));
        }

        [Test]
        public void ConcatHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RavenProjection.Empty.Concat((RavenProjectionHandler)null));
        }

        [Test]
        public void ConcatHandlersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => RavenProjection.Empty.Concat((RavenProjectionHandler[])null));
        }

        [Test]
        public void ConcatProjectionReturnsExpectedResult()
        {
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler4 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var projection = new RavenProjection(new[]
            {
                handler3,
                handler4
            });
            var sut = new RavenProjection(new[]
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
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RavenProjection(new[]
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
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler3 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler4 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RavenProjection(new[]
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
            var sut = RavenProjection.Empty;

            var result = sut.ToBuilder().Build().Handlers;

            Assert.That(result, Is.Empty);
        }

        [Test]
        public void ToBuilderReturnsExpectedResult()
        {
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var sut = new RavenProjection(new[]
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
        public void ImplicitConversionToRavenProjectionHandlerArray()
        {
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RavenProjection(handlers);

            RavenProjectionHandler[] result = sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }

        [Test]
        public void ExplicitConversionToRavenProjectionHandlerArray()
        {
            var handler1 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));
            var handler2 = new RavenProjectionHandler(typeof(object), (connection, message, token) => Task.FromResult(false));

            var handlers = new[]
            {
                handler1,
                handler2
            };

            var sut = new RavenProjection(handlers);

            var result = (RavenProjectionHandler[])sut;

            Assert.That(result, Is.EquivalentTo(handlers));
        }
    }
}