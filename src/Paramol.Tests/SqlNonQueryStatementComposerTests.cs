using System;
using System.Collections.Generic;
using System.Data.Common;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class SqlNonQueryStatementComposerTests
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
            Assert.Throws<ArgumentNullException>(() => sut.Compose((SqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeIfParamsArrayStatementsCanNotBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeIf(true, (SqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeIfParamsArrayStatementsCanBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeIf(false, (SqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeUnlessParamsArrayStatementsCanNotBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeUnless(false, (SqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeUnlessParamsArrayStatementsCanBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeUnless(true, (SqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeEnumerationStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose((IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeIfParamsEnumerationStatementsCanNotBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeIf(true, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeIfParamsEnumerationStatementsCanBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeIf(false, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeUnlessParamsEnumerationStatementsCanNotBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeUnless(false, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeUnlessParamsEnumerationStatementsCanBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeUnless(true, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeParamsArrayReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.Compose(
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeIfParamsArrayReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.ComposeIf(
                    condition,
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeUnlessParamsArrayReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.ComposeUnless(
                    condition,
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeEnumerationReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.Compose(
                    (IEnumerable<SqlNonQueryStatement>)new[] {
                        StatementFactory(),
                        StatementFactory()
                    }));
        }

        [Test]
        public void ComposeIfEnumerationReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.ComposeIf(
                    condition,
                    (IEnumerable<SqlNonQueryStatement>)new[] {
                        StatementFactory(),
                        StatementFactory()
                    }));
        }

        [Test]
        public void ComposeUnlessEnumerationReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                sut.ComposeUnless(
                    condition,
                    (IEnumerable<SqlNonQueryStatement>)new[] {
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

            SqlNonQueryStatement[] result = sut.Compose(statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedIfParamsArrayStatementsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeIf(true, statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedIfParamsArrayStatementsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeIf(false, statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedUnlessParamsArrayStatementsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeUnless(false, statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedUnlessParamsArrayStatementsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeUnless(true, statement3, statement4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
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

            SqlNonQueryStatement[] result = sut.Compose(
                (IEnumerable<SqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedIfEnumerationStatementsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeIf(
                true,
                (IEnumerable<SqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedIfEnumerationStatementsArePreservedAndReturnedByCompositionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeIf(
                false,
                (IEnumerable<SqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedUnlessEnumerationStatementsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeUnless(
                false,
                (IEnumerable<SqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2, statement3, statement4
            }));
        }

        [Test]
        public void ComposedUnlessEnumerationStatementsArePreservedAndReturnedByCompositionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            SqlNonQueryStatement[] result = sut.ComposeUnless(
                true,
                (IEnumerable<SqlNonQueryStatement>)new[] { statement3, statement4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ImplicitlyConvertsToTSqlStatementArray()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = new SqlNonQueryStatementComposer(new[] { statement1, statement2 });

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

            var result = (SqlNonQueryStatement[])SutFactory(statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        private static SqlNonQueryStatementComposer SutFactory(params SqlNonQueryStatement[] statements)
        {
            return new SqlNonQueryStatementComposer(statements);
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new DbParameter[0]);
        }
    }
}
