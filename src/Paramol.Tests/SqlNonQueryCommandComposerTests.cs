using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class SqlNonQueryCommandComposerTests
    {
        [Test]
        public void CtorStatementsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory(null));
        }

        [Test]
        public void ComposeParamsArrayCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose((SqlNonQueryCommand[])null));
        }

        [Test]
        public void ComposeIfParamsArrayCommandsCanNotBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeIf(true, (SqlNonQueryCommand[])null));
        }

        [Test]
        public void ComposeIfParamsArrayCommandsCanBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeIf(false, (SqlNonQueryCommand[])null));
        }

        [Test]
        public void ComposeUnlessParamsArrayCommandsCanNotBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeUnless(false, (SqlNonQueryCommand[])null));
        }

        [Test]
        public void ComposeUnlessParamsArrayCommandsCanBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeUnless(true, (SqlNonQueryCommand[])null));
        }

        [Test]
        public void ComposeEnumerationCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.Compose((IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfParamsEnumerationCommandsCanNotBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeIf(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfParamsEnumerationCommandsCanBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeIf(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessParamsEnumerationCommandsCanNotBeNullWhenConditionIsFalse()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(() => sut.ComposeUnless(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessParamsEnumerationCommandsCanBeNullWhenConditionIsTrue()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(() => sut.ComposeUnless(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeParamsArrayReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.Compose(
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeIfParamsArrayReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.ComposeIf(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeUnlessParamsArrayReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.ComposeUnless(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeEnumerationReturnsComposition()
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.Compose(
                    (IEnumerable<SqlNonQueryCommand>)new[] {
                        CommandFactory(),
                        CommandFactory()
                    }));
        }

        [Test]
        public void ComposeIfEnumerationReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.ComposeIf(
                    condition,
                    (IEnumerable<SqlNonQueryCommand>)new[] {
                        CommandFactory(),
                        CommandFactory()
                    }));
        }

        [Test]
        public void ComposeUnlessEnumerationReturnsComposition([Values(true, false)]bool condition)
        {
            var sut = SutFactory();
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                sut.ComposeUnless(
                    condition,
                    (IEnumerable<SqlNonQueryCommand>)new[] {
                        CommandFactory(),
                        CommandFactory()
                    }));
        }

        [Test]
        public void ComposedParamsArrayCommandsArePreservedAndReturnedByComposition()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.Compose(command3, command4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedIfParamsArrayCommandsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeIf(true, command3, command4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedIfParamsArrayCommandsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeIf(false, command3, command4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedUnlessParamsArrayCommandsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeUnless(false, command3, command4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedUnlessParamsArrayCommandsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeUnless(true, command3, command4);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedEnumerationCommandsArePreservedAndReturnedByComposition()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.Compose(
                (IEnumerable<SqlNonQueryCommand>)new[] { command3, command4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedIfEnumerationCommandsArePreservedAndReturnedByCompositionWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeIf(
                true,
                (IEnumerable<SqlNonQueryCommand>)new[] { command3, command4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedIfEnumerationCommandsArePreservedAndReturnedByCompositionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeIf(
                false,
                (IEnumerable<SqlNonQueryCommand>)new[] { command3, command4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedUnlessEnumerationCommandsArePreservedAndReturnedByCompositionWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeUnless(
                false,
                (IEnumerable<SqlNonQueryCommand>)new[] { command3, command4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2, command3, command4
            }));
        }

        [Test]
        public void ComposedUnlessEnumerationCommandsArePreservedAndReturnedByCompositionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var sut = SutFactory(command1, command2);

            var command3 = CommandFactory();
            var command4 = CommandFactory();

            SqlNonQueryCommand[] result = sut.ComposeUnless(
                true,
                (IEnumerable<SqlNonQueryCommand>)new[] { command3, command4 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ImplicitlyConvertsToTSqlCommandsArray()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = new SqlNonQueryCommandComposer(new[] { command1, command2 });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ExplicitlyConvertsToTSqlCommandsArray()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            var result = (SqlNonQueryCommand[])SutFactory(command1, command2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        private static SqlNonQueryCommandComposer SutFactory(params SqlNonQueryCommand[] statements)
        {
            return new SqlNonQueryCommandComposer(statements);
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text);
        }
    }
}
