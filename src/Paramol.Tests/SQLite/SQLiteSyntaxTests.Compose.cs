using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace Paramol.Tests.SQLite
{
    public partial class SQLiteSyntaxTests
    {
        [Test]
        public void ComposeCommandArrayCanNotBeNull()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => Sql.Compose((SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfCommandArrayCanNotBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => Sql.ComposeIf(true, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfCommandArrayCanBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => Sql.ComposeIf(false, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessCommandArrayCanNotBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => Sql.ComposeUnless(false, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessCommandArrayCanBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => Sql.ComposeUnless(true, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeCommandEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => Sql.Compose((IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfCommandEnumerationCanNotBeNullWhenConditionIsTrue()
        {
            Assert.Throws<ArgumentNullException>(() => Sql.ComposeIf(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfCommandEnumerationCanBeNullWhenConditionIsFalse()
        {
            Assert.DoesNotThrow(() => Sql.ComposeIf(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationCanNotBeNullWhenConditionIsFalse()
        {
            Assert.Throws<ArgumentNullException>(() => Sql.ComposeUnless(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationCanBeNullWhenConditionIsTrue()
        {
            Assert.DoesNotThrow(() => Sql.ComposeUnless(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeCommandArrayReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.Compose(
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeIfCommandArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.ComposeIf(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeUnlessCommandArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.ComposeUnless(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeCommandEnumerationReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.Compose((IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposeIfCommandEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.ComposeIf(condition, (IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                Sql.ComposeUnless(condition, (IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposedCommandArrayIsPreservedAndReturnedByComposer()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.Compose(command1, command2);

            Assert.That(result, Is.EquivalentTo(new []
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandArrayIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeIf(true, command1, command2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandArrayIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeIf(false, command1, command2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedUnlessCommandArrayIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeUnless(false, command1, command2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedUnlessCommandArrayIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeUnless(true, command1, command2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedCommandEnumerationIsPreservedAndReturnedByComposer()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.Compose((IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandEnumerationIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeIf(true, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeIf(false, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedUnlessCommandEnumerationIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeUnless(false, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedUnlessCommandEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = Sql.ComposeUnless(true, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text);
        }
    }
}
