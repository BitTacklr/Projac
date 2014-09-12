using System;
using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class SqlQueryCommandTests
    {
        [Test]
        public void TextCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory((string)null));
        }

        [Test]
        public void TextCanBeEmpty()
        {
            Assert.DoesNotThrow(() => SutFactory(""));
        }

        [Test]
        public void ParametersCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => SutFactory((DbParameter[])null));
        }

        [Test]
        public void TypeCanNotBeUnsupportedEnumValue()
        {
            Assert.Throws<ArgumentException>(() => SutFactory((CommandType)Int32.MinValue));
        }

        [TestCase(CommandType.StoredProcedure)]
        [TestCase(CommandType.TableDirect)]
        [TestCase(CommandType.Text)]
        public void TypeCanBeUnsupportedEnumValue(CommandType type)
        {
            Assert.DoesNotThrow(() => SutFactory(type));
        }

        [Test]
        public void ParametersCanBeEmpty()
        {
            Assert.DoesNotThrow(() => SutFactory(new DbParameter[0]));
        }

        [Test]
        public void PropertiesReturnExpectedValues()
        {
            var sut = SutFactory("text", new DbParameter[0], CommandType.TableDirect);

            Assert.That(sut.Text, Is.EqualTo("text"));
            Assert.That(sut.Parameters, Is.EqualTo(new DbParameter[0]));
            Assert.That(sut.Type, Is.EqualTo(CommandType.TableDirect));
        }

        private static SqlQueryCommand SutFactory(string text)
        {
            return SutFactory(text, new DbParameter[0], CommandType.Text);
        }

        private static SqlQueryCommand SutFactory(DbParameter[] parameters)
        {
            return SutFactory("text", parameters, CommandType.Text);
        }

        private static SqlQueryCommand SutFactory(CommandType type)
        {
            return SutFactory("text", new DbParameter[0], type);
        }

        private static SqlQueryCommand SutFactory(string text, DbParameter[] parameters, CommandType type)
        {
            return new SqlQueryCommand(text, parameters, type);
        }
    }
}