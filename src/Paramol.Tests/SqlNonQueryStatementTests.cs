using System;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Paramol.Tests
{
    [TestFixture]
    public class SqlNonQueryStatementTests
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
            Assert.Throws<ArgumentNullException>(() => SutFactory((SqlParameter[])null));
        }

        [Test]
        public void ParametersCanBeEmpty()
        {
            Assert.DoesNotThrow(() => SutFactory(new SqlParameter[0]));
        }

        [Test]
        public void ParameterCountLimitedTo2098()
        {
            Assert.Throws<ArgumentException>(() => SutFactory(new SqlParameter[2099]));
        }

        [Test]
        public void PropertiesReturnExpectedValues()
        {
            var sut = SutFactory("text", new SqlParameter[0]);

            Assert.That(sut.Text, Is.EqualTo("text"));
            Assert.That(sut.Parameters, Is.EqualTo(new SqlParameter[0]));
        }

        private static SqlNonQueryStatement SutFactory(string text)
        {
            return SutFactory(text, new SqlParameter[0]);
        }

        private static SqlNonQueryStatement SutFactory(SqlParameter[] parameters)
        {
            return SutFactory("text", parameters);
        }

        private static SqlNonQueryStatement SutFactory(string text, SqlParameter[] parameters)
        {
            return new SqlNonQueryStatement(text, parameters);
        }
    }
}
