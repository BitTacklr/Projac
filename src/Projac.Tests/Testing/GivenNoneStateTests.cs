using System;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class GivenNoneStateTests
    {
        private IScenarioGivenNoneStateBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Scenario(TSqlProjection.Empty).GivenNone();
        }

        [Test]
        public void WhenEventCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When(null));
        }

        [Test]
        public void GivensAreEmptyUponBuild()
        {
            var result = _sut.When(new object()).ExpectRowCount(TSql.Query(""), 0).Build().Givens;
            Assert.That(result, Is.Empty);
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