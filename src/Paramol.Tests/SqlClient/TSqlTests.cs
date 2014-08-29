using System;
using System.Collections.Generic;
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
        [Test]
        public void VarCharReturnsExpectedInstance()
        {
            Assert.That(TSql.VarChar("value", 123), Is.EqualTo(new TSqlVarCharValue("value", new TSqlVarCharSize(123))));
        }

        [Test]
        public void VarCharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarChar(null, 123), Is.EqualTo(new TSqlVarCharNullValue(new TSqlVarCharSize(123))));
        }

        [Test]
        public void CharReturnsExpectedInstance()
        {
            Assert.That(TSql.Char("value", 123), Is.EqualTo(new TSqlCharValue("value", new TSqlCharSize(123))));
        }

        [Test]
        public void CharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Char(null, 123), Is.EqualTo(new TSqlCharNullValue(new TSqlCharSize(123))));
        }

        [Test]
        public void VarCharMaxReturnsExpectedInstance()
        {
            Assert.That(TSql.VarCharMax("value"), Is.EqualTo(new TSqlVarCharValue("value", TSqlVarCharSize.Max)));
        }

        [Test]
        public void VarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarCharMax(null), Is.EqualTo(new TSqlVarCharNullValue(TSqlVarCharSize.Max)));
        }

        [Test]
        public void NVarCharReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarChar("value", 123), Is.EqualTo(new TSqlNVarCharValue("value", new TSqlNVarCharSize(123))));
        }

        [Test]
        public void NVarCharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarChar(null, 123), Is.EqualTo(new TSqlNVarCharNullValue(new TSqlNVarCharSize(123))));
        }

        [Test]
        public void NCharReturnsExpectedInstance()
        {
            Assert.That(TSql.NChar("value", 123), Is.EqualTo(new TSqlNCharValue("value", new TSqlNCharSize(123))));
        }

        [Test]
        public void NCharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.NChar(null, 123), Is.EqualTo(new TSqlNCharNullValue(new TSqlNCharSize(123))));
        }

        [Test]
        public void NVarCharMaxReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarCharMax("value"), Is.EqualTo(new TSqlNVarCharValue("value", TSqlNVarCharSize.Max)));
        }

        [Test]
        public void NVarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarCharMax(null), Is.EqualTo(new TSqlNVarCharNullValue(TSqlNVarCharSize.Max)));
        }

        [Test]
        public void BinaryReturnsExpectedInstance()
        {
            Assert.That(TSql.Binary(new byte[]{ 1,2,3}, 123), Is.EqualTo(new TSqlBinaryValue(new byte[] { 1, 2 , 3}, new TSqlBinarySize(123))));
        }

        [Test]
        public void BinaryNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Binary(null, 123), Is.EqualTo(new TSqlBinaryNullValue(new TSqlBinarySize(123))));
        }

        [Test]
        public void VarBinaryReturnsExpectedInstance()
        {
            Assert.That(TSql.VarBinary(new byte[] { 1, 2, 3 }, 123), Is.EqualTo(new TSqlVarBinaryValue(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123))));
        }

        [Test]
        public void VarBinaryNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarBinary(null, 123), Is.EqualTo(new TSqlVarBinaryNullValue(new TSqlVarBinarySize(123))));
        }

        [Test]
        public void VarBinaryMaxReturnsExpectedInstance()
        {
            Assert.That(TSql.VarBinaryMax(new byte[] { 1, 2, 3 }), Is.EqualTo(new TSqlVarBinaryValue(new byte[] { 1, 2, 3 }, TSqlVarBinarySize.Max)));
        }

        [Test]
        public void VarBinaryMaxNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarBinaryMax(null), Is.EqualTo(new TSqlVarBinaryNullValue(TSqlVarBinarySize.Max)));
        }

        [TestCaseSource("NonQueryFormatCases")]
        public void NonQueryFormatReturnsExpectedInstance(SqlNonQueryStatement actual, SqlNonQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryFormatCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryFormat("text"),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", parameters: null),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new IDbParameterValue[0]),
                new SqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestSqlParameter()),
                new SqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0}", new TestSqlParameter()),
                new SqlNonQueryStatement("text @P0", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0"),
                    new TestSqlParameter().ToDbParameter("@P1")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement("text @P0 @P1", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0"),
                    new TestSqlParameter().ToDbParameter("@P1")
                }));
        }

        [TestCaseSource("NonQueryFormatIfCases")]
        public void NonQueryFormatIfReturnsExpectedInstance(IEnumerable<SqlNonQueryStatement> actual, SqlNonQueryStatement[] expected)
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
                new[] {new SqlNonQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", parameters: null),
                new[] {new SqlNonQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new IDbParameterValue[0]),
                new[] {new SqlNonQueryStatement("text", new SqlParameter[0])});
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text {0}", new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text @P0", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(true, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text @P0 @P1", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });

            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text"),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", parameters: null),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text {0}", new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatIf(false, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
        }

        [TestCaseSource("NonQueryFormatUnlessCases")]
        public void NonQueryFormatUnlessReturnsExpectedInstance(IEnumerable<SqlNonQueryStatement> actual, SqlNonQueryStatement[] expected)
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
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text {0}", new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text @P0", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(false, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlNonQueryStatement("text @P0 @P1", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });

            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text"),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", parameters: null),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text {0}", new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
            yield return new TestCaseData(
                TSql.NonQueryFormatUnless(true, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new SqlNonQueryStatement[0]);
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
               new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", parameters: null),
                new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new IDbParameterValue[0]),
                new SqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestSqlParameter()),
                new SqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0}", new TestSqlParameter()),
                new SqlQueryStatement("text @P0", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0"),
                    new TestSqlParameter().ToDbParameter("@P1")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new SqlQueryStatement("text @P0 @P1", new[]
                {
                    new TestSqlParameter().ToDbParameter("@P0"),
                    new TestSqlParameter().ToDbParameter("@P1")
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
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text {0}", new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatIf(true, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0 @P1", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
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
                TSql.QueryFormatIf(false, "text", new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text {0}", new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatIf(false, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
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
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", parameters: null),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryStatement("text", new SqlParameter[0]) });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text {0}", new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
                    })
                });
            yield return new TestCaseData(
                TSql.QueryFormatUnless(false, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new[]
                {
                    new SqlQueryStatement("text @P0 @P1", new[]
                    {
                        new TestSqlParameter().ToDbParameter("@P0"),
                        new TestSqlParameter().ToDbParameter("@P1")
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
                TSql.QueryFormatUnless(true, "text", new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text {0}", new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text", new TestSqlParameter(), new TestSqlParameter()),
                new SqlQueryStatement[0]);
            yield return new TestCaseData(
                TSql.QueryFormatUnless(true, "text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new SqlQueryStatement[0]);
        }

        [Test]
        public void ComposeStatementArrayCanNotBeNull()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((SqlNonQueryStatement[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfStatementArrayCanNotBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeIf(true, (SqlNonQueryStatement[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeIfStatementArrayCanBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => TSql.ComposeIf(false, (SqlNonQueryStatement[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessStatementArrayCanNotBeNullWhenConditionIsFalse()
        {
            // ReSharper disable RedundantCast
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeUnless(false, (SqlNonQueryStatement[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeUnlessStatementArrayCanBeNullWhenConditionIsTrue()
        {
            // ReSharper disable RedundantCast
            Assert.DoesNotThrow(() => TSql.ComposeUnless(true, (SqlNonQueryStatement[])null));
            // ReSharper restore RedundantCast
        }

        [Test]
        public void ComposeStatementEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeIfStatementEnumerationCanNotBeNullWhenConditionIsTrue()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeIf(true, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeIfStatementEnumerationCanBeNullWhenConditionIsFalse()
        {
            Assert.DoesNotThrow(() => TSql.ComposeIf(false, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeUnlessStatementEnumerationCanNotBeNullWhenConditionIsFalse()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.ComposeUnless(false, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeUnlessStatementEnumerationCanBeNullWhenConditionIsTrue()
        {
            Assert.DoesNotThrow(() => TSql.ComposeUnless(true, (IEnumerable<SqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeStatementArrayReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.Compose(
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeIfStatementArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.ComposeIf(
                    condition,
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeUnlessStatementArrayReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.ComposeUnless(
                    condition,
                    StatementFactory(),
                    StatementFactory()));
        }

        [Test]
        public void ComposeStatementEnumerationReturnsComposer()
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.Compose((IEnumerable<SqlNonQueryStatement>)new[]
                {
                    StatementFactory(),
                    StatementFactory()
                }));
        }

        [Test]
        public void ComposeIfStatementEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.ComposeIf(condition, (IEnumerable<SqlNonQueryStatement>)new[]
                {
                    StatementFactory(),
                    StatementFactory()
                }));
        }

        [Test]
        public void ComposeUnlessStatementEnumerationReturnsComposer([Values(true, false)]bool condition)
        {
            Assert.IsInstanceOf<SqlNonQueryStatementComposer>(
                TSql.ComposeUnless(condition, (IEnumerable<SqlNonQueryStatement>)new[]
                {
                    StatementFactory(),
                    StatementFactory()
                }));
        }

        [Test]
        public void ComposedStatementArrayIsPreservedAndReturnedByComposer()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.Compose(statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new []
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedIfStatementArrayIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeIf(true, statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedIfStatementArrayIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeIf(false, statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryStatement[0]));
        }

        [Test]
        public void ComposedUnlessStatementArrayIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeUnless(false, statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedUnlessStatementArrayIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeUnless(true, statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryStatement[0]));
        }

        [Test]
        public void ComposedStatementEnumerationIsPreservedAndReturnedByComposer()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.Compose((IEnumerable<SqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedIfStatementEnumerationIsPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeIf(true, (IEnumerable<SqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedIfStatementEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeIf(false, (IEnumerable<SqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryStatement[0]));
        }

        [Test]
        public void ComposedUnlessStatementEnumerationIsPreservedAndReturnedByComposerWhenConditionIsFalse()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeUnless(false, (IEnumerable<SqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }


        [Test]
        public void ComposedUnlessStatementEnumerationIsNotPreservedAndReturnedByComposerWhenConditionIsTrue()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            SqlNonQueryStatement[] result = TSql.ComposeUnless(true, (IEnumerable<SqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new SqlNonQueryStatement[0]));
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new SqlParameter[0]);
        }

        class TestSqlParameter : IDbParameterValue
        {
            public DbParameter ToDbParameter(string parameterName)
            {
                return new SqlParameter(parameterName, DBNull.Value);
            }
        }
    }
}
