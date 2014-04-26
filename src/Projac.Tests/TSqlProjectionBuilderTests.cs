using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlProjectionBuilderTests
    {
        private TSqlProjectionBuilder _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TSqlProjectionBuilder();
        }

        [Test]
        public void InitialInstanceBuildReturnsExpectedResult()
        {
            var result = _sut.Build();

            Assert.That(result.Handlers, Is.Empty);
        }

        [Test]
        public void WhenHandlerWithSingleStatementCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, ITSqlStatement>) null));
        }


        [Test]
        public void WhenHandlerWithSingleStatementReturnsExpectedResult()
        {
            var result = _sut.When((object _) => new TSqlStatementStub());

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithSingleStatementIsPreservedUponBuild()
        {
            var statement = new TSqlStatementStub();
            Func<object, ITSqlStatement> handler = _ => statement;
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new ITSqlStatement[] { statement })), 
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, ITSqlStatement[]>)null));
        }

        [Test]
        public void WhenHandlerWithStatementArrayReturnsExpectedResult()
        {
            var result = _sut.When((object _) => new ITSqlStatement[] { new TSqlStatementStub(), new TSqlStatementStub() });

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementArrayIsPreservedUponBuild()
        {
            var statement1 = new TSqlStatementStub();
            var statement2 = new TSqlStatementStub();
            Func<object, ITSqlStatement[]> handler = _ => new ITSqlStatement[] { statement1, statement2 };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new ITSqlStatement[] { statement1, statement2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, IEnumerable<ITSqlStatement>>)null));
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationReturnsExpectedResult()
        {
            var result = _sut.When((object _) => (IEnumerable<ITSqlStatement>) new ITSqlStatement[]
            {
                new TSqlStatementStub(), new TSqlStatementStub()
            });

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationIsPreservedUponBuild()
        {
            var statement1 = new TSqlStatementStub();
            var statement2 = new TSqlStatementStub();
            Func<object, IEnumerable<ITSqlStatement>> handler = _ => (IEnumerable<ITSqlStatement>) new ITSqlStatement[]
            {
                statement1, statement2
            };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new ITSqlStatement[] { statement1, statement2 })),
                Is.EqualTo(1));
        }
    }
}
