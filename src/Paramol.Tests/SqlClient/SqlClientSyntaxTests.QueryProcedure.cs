using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Paramol.Tests.SqlClient
{
    public partial class SqlClientSyntaxTests
    {
        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureCases")]
        public void QueryProcedureReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureIfCases")]
        public void QueryProcedureIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureUnlessCases")]
        public void QueryProcedureUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureFormatCases")]
        public void QueryProcedureFormatReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureFormatIfCases")]
        public void QueryProcedureFormatIfReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        [TestCaseSource(typeof(SqlClientSyntaxTestCases), "QueryProcedureFormatUnlessCases")]
        public void QueryProcedureFormatUnlessReturnsExpectedInstance(IEnumerable<SqlQueryCommand> actual, SqlQueryCommand[] expected)
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
