using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Paramol.Tests.SQLite
{
    public partial class SQLiteSyntaxTests
    {
        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementCases")]
        public void QueryStatementReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SQLiteParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementIfCases")]
        public void QueryStatementIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementUnlessCases")]
        public void QueryStatementUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementFormatCases")]
        public void QueryStatementFormatReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SQLiteParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementFormatIfCases")]
        public void QueryStatementFormatIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SQLiteParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SQLiteSyntaxTestCases), "QueryStatementFormatUnlessCases")]
        public void QueryStatementFormatUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
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
