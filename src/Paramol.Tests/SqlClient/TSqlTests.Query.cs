using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;
using Paramol.Tests.Framework;

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [TestCaseSource("QueryCases")]
        public void QueryReturnsExpectedInstance(SqlQueryStatement actual, SqlQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryCases()
        {
            yield return new TestCaseData(
                TSql.Query("text"),
                new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", parameters: null),
                new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { }),
                new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter = new TestDbParameter() }),
                new SqlQueryStatement("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlQueryStatement("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter1"),
                    new TestDbParameter().ToDbParameter("@Parameter2")
                }));
        }

        [TestCaseSource("QueryIfCases")]
        public void QueryIfReturnsExpectedInstance(IEnumerable<SqlQueryStatement> actual, SqlQueryStatement[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> QueryIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryIf(true, "text"),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", new { }),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryIf(true, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
                    })
                });

            yield return new TestCaseData(
                TSql.QueryIf(false, "text"),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryIf(false, "text", parameters: null),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryIf(false, "text", new { }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryIf(false, "text", new { Parameter = new TestDbParameter() }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryIf(false, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlQueryStatement[0]);
        }

        [TestCaseSource("QueryUnlessCases")]
        public void QueryUnlessReturnsExpectedInstance(IEnumerable<SqlQueryStatement> actual, SqlQueryStatement[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> QueryUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text"),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", new { }),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
                    })
                });

            yield return new TestCaseData(
                TSql.QueryUnless(true, "text"),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryUnless(true, "text", parameters: null),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryUnless(true, "text", new { }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryUnless(true, "text", new { Parameter = new TestDbParameter() }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryUnless(true, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlQueryStatement[0]);
        }

        [TestCaseSource("QueryFormatCases")]
        public void QueryFormatReturnsExpectedInstance(SqlQueryStatement actual, SqlQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryFormatCases()
        {
            yield return new TestCaseData(
               TSql.QueryFormat("text"),
               new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", parameters: null),
                new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new IDbParameterValue[0]),
                new SqlQueryStatement("text", new DbParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestDbParameter()),
                new SqlQueryStatement("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0}", new TestDbParameter()),
                new SqlQueryStatement("text @P0", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement("text @P0 @P1", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
                }));
        }

        [TestCaseSource("QueryFormatIfCases")]
        public void QueryFormatIfReturnsExpectedInstance(IEnumerable<SqlQueryStatement> actual, SqlQueryStatement[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> QueryFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text"),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    })
                });

            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text"),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text", parameters: null),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text", new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text {0}", new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement[0]);
        }

        [TestCaseSource("QueryFormatUnlessCases")]
        public void QueryFormatUnlessReturnsExpectedInstance(IEnumerable<SqlQueryStatement> actual, SqlQueryStatement[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> QueryFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text"),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryStatement("text", new DbParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    })
                });

            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text"),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text", parameters: null),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text", new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text {0}", new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlQueryStatement[0]);
        }
    }
}
