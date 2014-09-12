using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;
using Paramol.Tests.Framework;

namespace Paramol.Tests.SqlClient
{
    [TestFixture]
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

        [TestCaseSource("NonQueryCases")]
        public void NonQueryReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryCases()
        {
            yield return new TestCaseData(
                TSql.NonQuery("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQuery("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { }),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter = new TestDbParameter() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@Parameter1"),
                    new TestDbParameter().ToDbParameter("@Parameter2")
                }, CommandType.Text));
        }

        [TestCaseSource("NonQueryIfCases")]
        public void NonQueryIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { Parameter = new TestDbParameter() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryUnlessCases")]
        public void NonQueryUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", new { Parameter = new TestDbParameter() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text",
                    new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@Parameter1"),
                        new TestDbParameter().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { Parameter = new TestDbParameter() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { Parameter1 = new TestDbParameter(), Parameter2 = new TestDbParameter() }),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryFormatCases")]
        public void NonQueryFormatReturnsExpectedInstance(SqlNonQueryCommand actual, SqlNonQueryCommand expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryFormatCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryFormat("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new IDbParameterValue[0]),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestDbParameter()),
                new SqlNonQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0}", new TestDbParameter()),
                new SqlNonQueryCommand("text @P0", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand("text", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand("text @P0 @P1", new[]
                {
                    new TestDbParameter().ToDbParameter("@P0"),
                    new TestDbParameter().ToDbParameter("@P1")
                }, CommandType.Text));
        }

        [TestCaseSource("NonQueryFormatIfCases")]
        public void NonQueryFormatIfReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));    
            }
        }

        private static IEnumerable<TestCaseData> NonQueryFormatIfCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text {0}", new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand[0]);
        }

        [TestCaseSource("NonQueryFormatUnlessCases")]
        public void NonQueryFormatUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryCommand> actual, SqlNonQueryCommand[] expected)
        {
            var actualArray = actual.ToArray();
            Assert.That(actualArray.Length, Is.EqualTo(expected.Length));
            for (var index = 0; index < actualArray.Length; index++)
            {
                Assert.That(actualArray[index].Text, Is.EqualTo(expected[index].Text));
                Assert.That(actualArray[index].Parameters, Is.EquivalentTo(expected[index].Parameters).Using(new SqlParameterEqualityComparer()));
            }
        }

        private static IEnumerable<TestCaseData> NonQueryFormatUnlessCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text {0}", new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new TestDbParameter().ToDbParameter("@P0"),
                        new TestDbParameter().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text {0}", new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text {0} {1}", new TestDbParameter(), new TestDbParameter()),
                new SqlNonQueryCommand[0]);
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

        [Test]
        public void ComposeCommandArrayCanNotBeNull()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfCommandArrayCanNotBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeIf(true, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfCommandArrayCanBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => TSql.ComposeIf(false, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessCommandArrayCanNotBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeUnless(false, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessCommandArrayCanBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => TSql.ComposeUnless(true, (SqlNonQueryCommand[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeCommandEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfCommandEnumerationCanNotBeNullWhenConditionIsTrue()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeIf(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeIfCommandEnumerationCanBeNullWhenConditionIsFalse()
        {
            Assert.DoesNotThrow(() => TSql.ComposeIf(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationCanNotBeNullWhenConditionIsFalse()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeUnless(false, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationCanBeNullWhenConditionIsTrue()
        {
            Assert.DoesNotThrow(() => TSql.ComposeUnless(true, (IEnumerable<SqlNonQueryCommand>)null));
        }

        [Test]
        public void ComposeCommandArrayReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.Compose(
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeIfCommandArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.ComposeIf(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeUnlessCommandArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.ComposeUnless(
                    condition,
                    CommandFactory(),
                    CommandFactory()));
        }

        [Test]
        public void ComposeCommandEnumerationReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.Compose((IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposeIfCommandEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.ComposeIf(condition, (IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposeUnlessCommandEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryCommandComposer>(
                TSql.ComposeUnless(condition, (IEnumerable<SqlNonQueryCommand>)new[]
                {
                    CommandFactory(),
                    CommandFactory()
                }));
        }

        [Test]
        public void ComposedCommandArrayIsPreservedAndReturnedByComposer()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.Compose(command1, command2);

            Assert.That(result, Is.EquivalentTo(new []
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandArrayIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeIf(true, command1, command2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandArrayIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeIf(false, command1, command2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedUnlessCommandArrayIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeUnless(false, command1, command2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedUnlessCommandArrayIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeUnless(true, command1, command2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedCommandEnumerationIsPreservedAndReturnedByComposer()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.Compose((IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandEnumerationIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeIf(true, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }

        [Test]
        public void ComposedIfCommandEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeIf(false, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        [Test]
        public void ComposedUnlessCommandEnumerationIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeUnless(false, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                command1, command2
            }));
        }


        [Test]
        public void ComposedUnlessCommandEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();

            SqlNonQueryCommand[] result = TSql.ComposeUnless(true, (IEnumerable<SqlNonQueryCommand>)new[]
            {
                command1, command2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryCommand[0]));
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text);
        }

        class TestDbParameter : IDbParameterValue
        {
            public DbParameter ToDbParameter(string parameterName)
            {
                return new SqlParameter(parameterName, DBNull.Value);
            }
        }
    }
}
