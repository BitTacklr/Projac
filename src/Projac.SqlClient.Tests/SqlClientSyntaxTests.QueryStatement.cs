using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Projac.Sql.Tests.SqlClient
{
    public partial class SqlClientSyntaxTests
    {
        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementCases))]
        public void QueryStatementReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementIfCases))]
        public void QueryStatementIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementUnlessCases))]
        public void QueryStatementUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementFormatCases))]
        public void QueryStatementFormatReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementFormatIfCases))]
        public void QueryStatementFormatIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), nameof(SqlClientSyntaxTestCases.QueryStatementFormatUnlessCases))]
        public void QueryStatementFormatUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }
    }
}
