using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Projac.Tests.Framework;

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
        public void ComposeStatementsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose(null));
        }

        [Test]
        public void ComposeReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<TSqlStatementComposer>(
                sut.Compose(
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposedStatementsArePreservedAndReturnedByComposition()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            var sut = SutFactory(statement1, statement2);

            var statement3 = StatementFactory();
            var statement4 = StatementFactory();

            ITSqlStatement[] result = sut.Compose(statement3, statement4);

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

            ITSqlStatement[] result = new TSqlStatementComposer(new[] { statement1, statement2 });

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

            var result = (ITSqlStatement[])TSql.Compose(statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        private static TSqlStatementComposer SutFactory(params ITSqlStatement[] statements)
        {
            return new TSqlStatementComposer(statements);
        }

        private static ITSqlStatement StatementFactory()
        {
            return new TSqlStatementStub();
        }
    }
}
