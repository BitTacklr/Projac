using System;
using System.Collections.Generic;
using NUnit.Framework;
using Projac.Testing;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class GivenNoneWhenStateTests : WhenStateFixture
    {
        protected override IScenarioWhenStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).GivenNone().When(new object());
        }
    }

    [TestFixture]
    public class GivenEnumerableWhenStateTests : WhenStateFixture
    {
        protected override IScenarioWhenStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).Given((IEnumerable<object>)new object[0]).When(new object());
        }
    }

    [TestFixture]
    public class GivenArrayWhenStateTests : WhenStateFixture
    {
        protected override IScenarioWhenStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).Given(new object[0]).When(new object());
        }
    }

    public abstract class WhenStateFixture
    {
        private IScenarioWhenStateBuilder _sut;

        protected abstract IScenarioWhenStateBuilder SutFactory();

        [SetUp]
        public void SetUp()
        {
            _sut = SutFactory();
        }

        [Test]
        public void ExpectRowCountQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectRowCount(null, 0));
        }

        [Test]
        public void ExpectNonEmptyResultQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectNonEmptyResultSet(null));
        }

        [Test]
        public void ExpectEmptyResultQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectEmptyResultSet(null));
        }
    }
}