using System;
using System.Data;
using System.Data.Common;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class SqlNonQueryStatementTests
    {
        [Test]
        public void IsSqlNonQueryCommand()
        {
            Assert.That(SutFactory(), Is.InstanceOf<SqlNonQueryCommand>());
        }

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
        public void ParametersCanBeEmpty()
        {
            Assert.DoesNotThrow(() => SutFactory(new DbParameter[0]));
        }

        [Test]
        public void ParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => SutFactory(new DbParameter[2099]));
        }

        [Test]
        public void TypeReturnsExpectedResult()
        {
            var result = SutFactory().Type;
            Assert.That(result, Is.EqualTo(CommandType.Text));
        }

        private static SqlNonQueryStatement SutFactory()
        {
            return SutFactory("text", new DbParameter[0]);
        }

        private static SqlNonQueryStatement SutFactory(string text)
        {
            return SutFactory(text, new DbParameter[0]);
        }

        private static SqlNonQueryStatement SutFactory(DbParameter[] parameters)
        {
            return SutFactory("text", parameters);
        }

        private static SqlNonQueryStatement SutFactory(string text, DbParameter[] parameters)
        {
            return new SqlNonQueryStatement(text, parameters);
        }
    }
}
