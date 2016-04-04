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
        [TestCaseSource("NonQueryProcedureCases")]
        public void NonQueryProcedureReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedure("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedure("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedure("text", new { }),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedure("text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedure("text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                    new SqlParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.StoredProcedure));
        }

        [TestCaseSource("NonQueryProcedureIfCases")]
        public void NonQueryProcedureIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(true, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(true, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(false, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureIf(false, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryProcedureUnlessCases")]
        public void NonQueryProcedureUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(false, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(false, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(true, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureUnless(true, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryProcedureFormatCases")]
        public void NonQueryProcedureFormatReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureFormatCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text", new IDbParameterValue[0]),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text", new SqlParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand("text @P0", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.StoredProcedure));
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormat("text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand("text @P0 @P1", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.StoredProcedure));
        }

        [TestCaseSource("NonQueryProcedureFormatIfCases")]
        public void NonQueryProcedureFormatIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatIf(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryProcedureFormatUnlessCases")]
        public void NonQueryProcedureFormatUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryProcedureFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.StoredProcedure) });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.StoredProcedure)
                });

            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryProcedureFormatUnless(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }
    }
}
