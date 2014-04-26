using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;

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
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, TSqlNonQueryStatement>)null));
        }


        [Test]
        public void WhenHandlerWithSingleStatementReturnsExpectedResult()
        {
            var result = _sut.When((object _) => StatementFactory());

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithSingleStatementIsPreservedUponBuild()
        {
            var statement = StatementFactory();
            Func<object, TSqlNonQueryStatement> handler = _ => statement;
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new[] { statement })), 
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithSingleStatementPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var statements = new[]
            {
                StatementFactory(),
                StatementFactory()
            };
            var statement = StatementFactory();
            Func<object, TSqlNonQueryStatement> handler = _ => statement;
            var result = _sut.When((object _) => statements).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(statements)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, TSqlNonQueryStatement[]>)null));
        }

        [Test]
        public void WhenHandlerWithStatementArrayReturnsExpectedResult()
        {
            var result = _sut.When((object _) => new[] { StatementFactory(), StatementFactory() });

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementArrayIsPreservedUponBuild()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            Func<object, TSqlNonQueryStatement[]> handler = _ => new[] { statement1, statement2 };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new[] { statement1, statement2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementArrayPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var statements = new[]
            {
                StatementFactory(),
                StatementFactory()
            };
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            Func<object, TSqlNonQueryStatement[]> handler = _ => new[] { statement1, statement2 };
            var result = _sut.When((object _) => statements).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(statements)),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.When((Func<object, IEnumerable<TSqlNonQueryStatement>>)null));
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationReturnsExpectedResult()
        {
            var result = _sut.When((object _) => (IEnumerable<TSqlNonQueryStatement>) new []
            {
                StatementFactory(), StatementFactory()
            });

            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationIsPreservedUponBuild()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            Func<object, IEnumerable<TSqlNonQueryStatement>> handler = _ => (IEnumerable<TSqlNonQueryStatement>) new []
            {
                statement1, statement2
            };
            var result = _sut.When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(new[] { statement1, statement2 })),
                Is.EqualTo(1));
        }

        [Test]
        public void WhenHandlerWithStatementEnumerationPreservesPreviouslyCollectedStatementsUponBuild()
        {
            var statements = new[]
            {
                StatementFactory(),
                StatementFactory()
            };
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();
            Func<object, IEnumerable<TSqlNonQueryStatement>> handler = _ => (IEnumerable<TSqlNonQueryStatement>)new[]
            {
                statement1, statement2
            };
            var result = _sut.When((object _) => statements).When(handler).Build();

            Assert.That(
                result.Handlers.Count(_ => _.Event == typeof(object) && _.Handler(null).SequenceEqual(statements)),
                Is.EqualTo(1));
        }

        private static TSqlNonQueryStatement StatementFactory()
        {
            return new TSqlNonQueryStatement("text", new SqlParameter[0]);
        }
    }
}
