using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;
using Paramol.Tests.Framework;

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [Test]
        public void QueryStatementParameterCollectorPerformance()
        {
            var random = new Random();
            var watch = Stopwatch.StartNew();
            var results = new List<long>();
            const int collections = 10000;
            for(var run = 0; run < collections; run++)
            {
                watch.Restart();
                var _ = TSql.QueryStatement("text",
                    new
                    {
                        P1 = TSql.UniqueIdentifier(Guid.NewGuid()),
                        P2 = TSql.Int(random.Next()),
                        P3 = "", // ignored
                        P4 = 0 // ignored
                    });
                results.Add(watch.ElapsedTicks);
            }
            Assert.Pass("{0} collections took {1} ticks on average.", collections, results.Average());
        }

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
                TSql.QueryStatement("text", new { Parameter = new TestDbParameter() }),
                new SqlQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatement("text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter1"),
                    new TestDbParameter().ToDbParameter("@Parameter2")
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
                TSql.QueryStatementIf(true, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementIf(true, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
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
                TSql.QueryStatementIf(false, "text", new { Parameter = new TestDbParameter() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementIf(false, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
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
                TSql.QueryStatementUnless(false, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementUnless(false, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
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
                TSql.QueryStatementUnless(true, "text", new { Parameter = new TestDbParameter() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementUnless(true, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
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
                TSql.QueryStatementFormat("text", new TestDbParameter()),
                new SqlQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text {0}", new TestDbParameter()),
                new SqlQueryCommand("text @P0", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.QueryStatementFormat("text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryCommand("text @P0 @P1", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
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
                TSql.QueryStatementFormatIf(true, "text", new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
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
                TSql.QueryStatementFormatIf(false, "text", new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text {0}", new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatIf(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
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
                TSql.QueryStatementFormatUnless(false, "text", new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
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
                TSql.QueryStatementFormatUnless(true, "text", new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text {0}", new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                TSql.QueryStatementFormatUnless(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryCommand[0]);
        }
    }
}
