using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [TestCaseSource("QueryProcedureCases")]
        public void QueryProcedureReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryProcedureCases()
        {

            yield return new TestCaseData(
                TSql.QueryProcedure("text"),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedure("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedure("text", new { }),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedure("text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedure("text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                    new SqlParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.StoredProcedure));
        }

        [TestCaseSource("QueryProcedureIfCases")]
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

        private static IEnumerable<TestCaseData> QueryProcedureIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryProcedureIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureIf(true, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureIf(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureIf(true, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.QueryProcedureIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureIf(false, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureIf(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureIf(false, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryProcedureUnlessCases")]
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

        private static IEnumerable<TestCaseData> QueryProcedureUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(false, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(false, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.QueryProcedureUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(true, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureUnless(true, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryProcedureFormatCases")]
        public void QueryProcedureFormatReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryProcedureFormatCases()
        {
            yield return new TestCaseData(
               TSql.QueryProcedureFormat("text"),
               new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text", new IDbParameterValue[0]),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text", new SqlParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand("text @P0", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.QueryProcedureFormat("text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand("text @P0 @P1", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.StoredProcedure));
        }

        [TestCaseSource("QueryProcedureFormatIfCases")]
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

        private static IEnumerable<TestCaseData> QueryProcedureFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatIf(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryProcedureFormatUnlessCases")]
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

        private static IEnumerable<TestCaseData> QueryProcedureFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryProcedureFormatUnless(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
        }
    }
}
