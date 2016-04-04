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
        [TestCaseSource("NonQueryStatementCases")]
        public void NonQueryStatementReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryStatementCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatement("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatement("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatement("text", new { }),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatement("text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatement("text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                    new SqlParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.Text));
        }

        [TestCaseSource("NonQueryStatementIfCases")]
        public void NonQueryStatementIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryStatementIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(true, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(true, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryStatementIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(false, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementIf(false, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryStatementUnlessCases")]
        public void NonQueryStatementUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryStatementUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(false, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(false, "text", new { Parameter = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(false, "text",
                    new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@Parameter1"),
                        new SqlParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(true, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(true, "text", new { Parameter = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementUnless(true, "text", new { Parameter1 = new SqlParameterValueStub(), Parameter2 = new SqlParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryStatementFormatCases")]
        public void NonQueryStatementFormatReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryStatementFormatCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text", new IDbParameterValue[0]),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text", new SqlParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand("text @P0", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryStatementFormat("text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand("text @P0 @P1", new[]
                {
                    new SqlParameterValueStub().ToDbParameter("@P0"),
                    new SqlParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
        }

        [TestCaseSource("NonQueryStatementFormatIfCases")]
        public void NonQueryStatementFormatIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryStatementFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatIf(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryStatementFormatUnlessCases")]
        public void NonQueryStatementFormatUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryStatementFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text {0}", new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(false, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SqlParameterValueStub().ToDbParameter("@P0"),
                        new SqlParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text {0}", new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryStatementFormatUnless(true, "text {0} {1}", new SqlParameterValueStub(), new SqlParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }
    }
}
