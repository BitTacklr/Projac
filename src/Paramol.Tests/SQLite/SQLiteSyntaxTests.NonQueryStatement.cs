using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Paramol.Tests.SQLite
{
    public partial class SQLiteSyntaxTests
    {
        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementCases")]
        public void NonQueryStatementReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SQLiteParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementIfCases")]
        public void NonQueryStatementIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementUnlessCases")]
        public void NonQueryStatementUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementFormatCases")]
        public void NonQueryStatementFormatReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SQLiteParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementFormatIfCases")]
        public void NonQueryStatementFormatIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "NonQueryStatementFormatUnlessCases")]
        public void NonQueryStatementFormatUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }
    }
}
