using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Paramol.SqlClient;
using Paramol.Tests.Framework;

#if !FSHARP

namespace Paramol.Tests.SqlClient
{
    public partial class TSqlTests
    {
        [Test]
        public void BigIntReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(123), Is.EqualTo(new TSqlBigIntValue(123)));
        }

        [Test]
        public void BigIntNullReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(null), Is.EqualTo(TSqlBigIntNullValue.Instance));
        }

        [Test]
        public void IntReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(123), Is.EqualTo(new TSqlIntValue(123)));
        }

        [Test]
        public void IntNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(null), Is.EqualTo(TSqlIntNullValue.Instance));
        }

        [Test]
        public void BitReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(true), Is.EqualTo(new TSqlBitValue(true)));
        }

        [Test]
        public void BitNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(null), Is.EqualTo(TSqlBitNullValue.Instance));
        }

        [Test]
        public void UniqueIdentifierReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(Guid.Empty), Is.EqualTo(new TSqlUniqueIdentifierValue(Guid.Empty)));
        }

        [Test]
        public void UniqueIdentifierNullReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(null), Is.EqualTo(TSqlUniqueIdentifierNullValue.Instance));
        }

        [Test]
        public void DateTimeOffsetReturnsExpectedInstance()
        {
            var value = DateTimeOffset.UtcNow;
            Assert.That(TSql.DateTimeOffset(value), Is.EqualTo(new TSqlDateTimeOffsetValue(value)));
        }

        [Test]
        public void DateTimeOffsetNullReturnsExpectedInstance()
        {
            Assert.That(TSql.DateTimeOffset(null), Is.EqualTo(TSqlDateTimeOffsetNullValue.Instance));
        }

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
                new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", parameters: null),
                new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { }),
                new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter = new TestSqlParameter() }),
                new SqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new SqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@Parameter1"),
                    new TestSqlParameter().ToDbParameter("@Parameter2")
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
                new[] {new SqlQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", parameters: null),
                new[] {new SqlQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", new {}),
                new[] {new SqlQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.QueryIf(true, "text", new {Parameter = new TestSqlParameter()}),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryIf(true, "text",
                    new {Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter()}),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter1"),
                        new TestSqlParameter().ToDbParameter("@Parameter2")
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
                TSql.QueryIf(false, "text", new { Parameter = new TestSqlParameter() }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryIf(false, "text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
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
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", new { }),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text", new { Parameter = new TestSqlParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryUnless(false, "text",
                    new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter1"),
                        new TestSqlParameter().ToDbParameter("@Parameter2")
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
                TSql.QueryUnless(true, "text", new { Parameter = new TestSqlParameter() }),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryUnless(true, "text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new SqlQueryStatement[0]);
        }

        [TestCaseSource("NonQueryCases")]
        public void NonQueryReturnsExpectedInstance(SqlNonQueryStatement actual, SqlNonQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryCases()
        {
            yield return new TestCaseData(
                TSql.NonQuery("text"),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", parameters: null),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { }),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter = new TestSqlParameter() }),
                new SqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new SqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@Parameter1"),
                    new TestSqlParameter().ToDbParameter("@Parameter2")
                }));
        }

        [TestCaseSource("NonQueryIfCases")]
        public void NonQueryIfReturnsExpectedInstance(IEnumerable<SqlNonQueryStatement> actual, SqlNonQueryStatement[] expected)
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
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", parameters: null),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", new { }),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text", new { Parameter = new TestSqlParameter() }),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryIf(true, "text",
                    new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter1"),
                        new TestSqlParameter().ToDbParameter("@Parameter2")
                    })
                });

            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text"),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", parameters: null),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { }),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { Parameter = new TestSqlParameter() }),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryIf(false, "text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new SqlNonQueryStatement[0]);
        }

        [TestCaseSource("NonQueryUnlessCases")]
        public void NonQueryUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryStatement> actual, SqlNonQueryStatement[] expected)
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
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", new { }),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text", new { Parameter = new TestSqlParameter() }),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryUnless(false, "text",
                    new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@Parameter1"),
                        new TestSqlParameter().ToDbParameter("@Parameter2")
                    })
                });

            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text"),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", parameters: null),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { }),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { Parameter = new TestSqlParameter() }),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryUnless(true, "text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new SqlNonQueryStatement[0]);
        }
    }
}
#endif