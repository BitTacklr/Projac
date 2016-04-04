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
        [TestCaseSource("QueryStatementCases")]
        public void QueryStatementReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryStatementCases()
        {
            yield return new TestCaseData(
                TSql.QueryStatement("text"),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatement("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatement("text", new { }),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatement("text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatement("text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                    new SqlParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.Text));
        }

        [TestCaseSource("QueryStatementIfCases")]
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

        private static IEnumerable<TestCaseData> QueryStatementIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryStatementUnlessCases")]
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

        private static IEnumerable<TestCaseData> QueryStatementUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryStatementFormatCases")]
        public void QueryStatementFormatReturnsExpectedInstance(SqlQueryCommand actual, SqlQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryStatementFormatCases()
        {
            yield return new TestCaseData(
               TSql.QueryStatementFormat("text"),
               new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text", new IDbParameterValue[0]),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text", new SqlParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand("text @P0", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand("text @P0 @P1", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
        }

        [TestCaseSource("QueryStatementFormatIfCases")]
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

        private static IEnumerable<TestCaseData> QueryStatementFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
        }

        [TestCaseSource("QueryStatementFormatUnlessCases")]
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

        private static IEnumerable<TestCaseData> QueryStatementFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text {0}", new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlQueryCommand[0]);
        }
    }
}
