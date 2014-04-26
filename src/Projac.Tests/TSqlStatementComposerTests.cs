using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlStatementComposerTests
    {
        [Test]
        public void CtorStatementsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null));
        }

        [Test]
        public void ComposeParamsArrayStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose((TSqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeEnumerationStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose((IEnumerable<TSqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeParamsArrayReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<TSqlNonQueryStatementComposer>(
                sut.Compose(
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeEnumerationReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<TSqlNonQueryStatementComposer>(
                sut.Compose(
                    (IEnumerable<TSqlNonQueryStatement>)new[] {
                        StatementFactory(),
                        StatementFactory()
                    }));
        }

        [Test]
        public void ComposedParamsArrayStatementsArePreservedAndReturnedByComposition()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            TSqlNonQueryStatement[] result = sut.Compose(statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedEnumerationStatementsArePreservedAndReturnedByComposition()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            TSqlNonQueryStatement[] result = sut.Compose(
                (IEnumerable<TSqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ImplicitlyConvertsToTSqlStatementArray()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            TSqlNonQueryStatement[] result = new TSqlNonQueryStatementComposer(new[] { statement1, statement2 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ExplicitlyConvertsToTSqlStatementArray()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var result = (TSqlNonQueryStatement[])TSql.Compose(statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        private static TSqlNonQueryStatementComposer SutFactory(params TSqlNonQueryStatement[] statements)
        {
            return new TSqlNonQueryStatementComposer(statements);
        }

        private static TSqlNonQueryStatement StatementFactory()
        {
            return new TSqlNonQueryStatement("text", new SqlParameter[0]);
        }
    }
}
