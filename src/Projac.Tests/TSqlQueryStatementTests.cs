using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlQueryStatementTests
    {
        [Test]
        public void IsTSqlStatement()
        {
            Assert.IsInstanceOf<ITSqlStatement>(SutFactory("", new SqlParameter[0]));
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


        [Test]
        public void WriteToCommandCanNotBeNull()
        {
            var sut = SutFactory();

            Assert.Throws<ArgumentNullException>(() => sut.WriteTo(null));
        }

        [Test]
        public void WriteToCommandClearsCommandParameters()
        {
            var sut = SutFactory();

            var command = new SqlCommand();
            command.Parameters.Add(new SqlParameter());
            sut.WriteTo(command);

            Assert.That(command.Parameters, Is.Empty);
        }

        [Test]
        public void WriteToCommandAddsStatementParameters()
        {
            var parameter1 = new SqlParameter();
            var parameter2 = new SqlParameter();
            var sut = SutFactory(new[] { parameter1, parameter2 });

            var command = new SqlCommand();
            sut.WriteTo(command);

            Assert.That(command.Parameters, Is.EquivalentTo(new[] { parameter1, parameter2 }));
        }

        [Test]
        public void WriteToCommandSetsCommandText()
        {
            var sut = SutFactory("text");

            var command = new SqlCommand();
            sut.WriteTo(command);

            Assert.That(command.CommandText, Is.EqualTo("text"));
        }

        [Test]
        public void WriteToCommandSetsCommandType()
        {
            var sut = SutFactory();

            var command = new SqlCommand { CommandType = CommandType.StoredProcedure };
            sut.WriteTo(command);

            Assert.That(command.CommandType, Is.EqualTo(CommandType.Text));
        }

        private static TSqlQueryStatement SutFactory()
        {
            return SutFactory("text", new SqlParameter[0]);
        }

        private static TSqlQueryStatement SutFactory(string text)
        {
            return SutFactory(text, new SqlParameter[0]);
        }

        private static TSqlQueryStatement SutFactory(SqlParameter[] parameters)
        {
            return SutFactory("text", parameters);
        }

        private static TSqlQueryStatement SutFactory(string text, SqlParameter[] parameters)
        {
            return new TSqlQueryStatement(text, parameters);
        }
    }
}