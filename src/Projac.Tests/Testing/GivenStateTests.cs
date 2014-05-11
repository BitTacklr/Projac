using System;
using System.Collections.Generic;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class GivenEnumerableGivenStateTests :  GivenStateFixture
    {
        protected override IScenarioGivenStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).Given((IEnumerable<object>)new object[0]);
        }
    }

    [TestFixture]
    public class GivenArrayGivenStateTests : GivenStateFixture
    {
        protected override IScenarioGivenStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).Given(new object[0]);
        }
    }

    public abstract class GivenStateFixture
    {
        private IScenarioGivenStateBuilder _sut;

        protected abstract IScenarioGivenStateBuilder SutFactory();

        [SetUp]
        public void SetUp()
        {
            _sut = SutFactory();
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
        public void GivenEnumerableGivensArePreservedUponBuild()
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
        public void GivenArrayGivensArePreservedUponBuild()
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
        public void WhenEventIsPreservedUponBuild()
        {
            var @event = new object();
            var result = _sut.When(@event).ExpectRowCount(TSql.Query(""), 0).Build().When;
            Assert.That(result, Is.EqualTo(@event));
        }
    }
}