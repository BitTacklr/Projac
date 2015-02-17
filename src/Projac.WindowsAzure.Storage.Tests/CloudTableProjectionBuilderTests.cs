using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;

namespace Projac.WindowsAzure.Storage.Tests
{
    [TestFixture]
    public class CloudTableProjectionBuilderTests
    {
        private CloudTableProjectionBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CloudTableProjectionBuilder();
        }

        [Test]
        public void DecoratedProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CloudTableProjectionBuilder(null));
        }

        [Test]
        public void DecoratedProjectionHandlersAreCopiedOnConstruction()
        {
            var handler1 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var handler2 = new CloudTableProjectionHandler(typeof(object), (client, message, token) => Task.FromResult(false));
            var projection = new CloudTableProjection(new[]
            {
                handler1, 
                handler2
            });
            var sut = new CloudTableProjectionBuilder(projection);

            var result = sut.Build();

            Assert.That(result.Handlers, Is.EquivalentTo(new[]
            {
                handler1, handler2
            }));

        }

        [Test]
        public void InitialInstanceBuildReturnsExpectedResult()
        {
            var result = _sut.Build();

            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void WhenHandlerCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When<object>((Func<CloudTableClient, object, Task>)null));
        }

        [Test]
        public void WhenHandlerWithTokenCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When<object>((Func<CloudTableClient, object, CancellationToken, Task>)null));
        }

        [Test]
        public void WhenHandlerReturnsExpectedResult()
        {
            var result = _sut.When<object>((client, message) => Task.FromResult(0));

            Assert.That(result, Is.InstanceOf<CloudTableProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerRetainsMessageType()
        {
            _sut.When<Message>((client, message) =>
            {
                Message _ = message;
                return Task.FromResult(0);
            });
        }

        class Message { }

        [Test]
        public void WhenHandlerWithTokenReturnsExpectedResult()
        {
            var result = _sut.When<object>((client, message, token) => Task.FromResult(0));

            Assert.That(result, Is.InstanceOf<CloudTableProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithTokenRetainsMessageType()
        {
            _sut.When<Message>((client, message, token) =>
            {
                Message _ = message;
                return Task.FromResult(0);
            });
        }

        [Test]
        public void WhenHandlerIsPreservedUponBuild()
        {
            var task = Task.FromResult(false);
            Func<CloudTableClient, object, Task> handler = (client, message) => task;
            var result = _sut.When<object>(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => 
                    _.Message == typeof(object) && 
                    ReferenceEquals(_.Handler(null, null, CancellationToken.None), task)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithTokenIsPreservedUponBuild()
        {
            var task = Task.FromResult(false);
            Func<CloudTableClient, object, CancellationToken, Task> handler = (client, message, token) => task;
            var result = _sut.When<object>(handler).Build();

            Assert.That(
                result.Handlers.Count(_ =>
                    _.Message == typeof(object) &&
                    ReferenceEquals(_.Handler(null, null, CancellationToken.None), task)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerPreservesPreviouslyCollectedHandlersUponBuild()
        {
            var task1 = Task.FromResult(false);
            var task2 = Task.FromResult(false);
            Func<CloudTableClient, object, Task> handler1 = (client, message) => task1;
            Func<CloudTableClient, object, Task> handler2 = (client, message) => task2;
            var result = _sut.When<object>(handler1).When<object>(handler2).Build();

            Assert.That(
                result.Handlers.Length,
                Is.EqualTo(2));

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && ReferenceEquals(_.Handler(null, null, CancellationToken.None), task1)),
                Is.EqualTo(1));

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && ReferenceEquals(_.Handler(null, null, CancellationToken.None), task2)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithTokenPreservesPreviouslyCollectedHandlersUponBuild()
        {
            var task1 = Task.FromResult(false);
            var task2 = Task.FromResult(false);
            Func<CloudTableClient, object, CancellationToken, Task> handler1 = (client, message, token) => task1;
            Func<CloudTableClient, object, CancellationToken, Task> handler2 = (client, message, token) => task2;
            var result = _sut.When<object>(handler1).When<object>(handler2).Build();

            Assert.That(
                result.Handlers.Length,
                Is.EqualTo(2));

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && ReferenceEquals(_.Handler(null, null, CancellationToken.None), task1)),
                Is.EqualTo(1));

            Assert.That(
                result.Handlers.Count(_ => _.Message == typeof(object) && ReferenceEquals(_.Handler(null, null, CancellationToken.None), task2)),
                Is.EqualTo(1));
        }
    }
}