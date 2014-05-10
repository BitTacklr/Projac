using System;
using System.Collections.Generic;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class ScenarioTests
    {
        private Scenario _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Scenario(TSqlProjection.Empty);
        }

        [Test]
        public void IsInitialStateBuilder()
        {
            Assert.IsInstanceOf<IScenarioInitialStateBuilder>(_sut);
        }

        [Test]
        public void ProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => new Scenario(null));
        }

        [Test]
        public void GivenEnumerableCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Given((IEnumerable<object>)null));
        }

        [Test]
        public void GivenArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Given((object[])null));
        }

        [Test]
        public void WhenEventCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When(null));
        }

        [Test]
        public void GivenEnumerableGivensArePreserved()
        {
            IEnumerable<object> givens = new[]
            {
                new object(),
                new object()
            };
            var result = _sut.Given(givens).When(new object()).ExpectRowCount(TSql.Query(""), 0).Build().Givens;
            Assert.That(result, Is.EquivalentTo(givens));
        }

        [Test]
        public void GivenArrayGivensArePreserved()
        {
            var givens = new[]
            {
                new object(),
                new object()
            };
            var result = _sut.Given(givens).When(new object()).ExpectRowCount(TSql.Query(""), 0).Build().Givens;
            Assert.That(result, Is.EquivalentTo(givens));
        }

        [Test]
        public void GivenNoneNoGivensArePreserved()
        {
            var result = _sut.GivenNone().When(new object()).ExpectRowCount(TSql.Query(""), 0).Build().Givens;
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void WhenEventIsPreserved()
        {
            var @event = new object();
            var result = _sut.When(@event).ExpectRowCount(TSql.Query(""), 0).Build().When;
            Assert.That(result, Is.EqualTo(@event));
        }
    }
}
