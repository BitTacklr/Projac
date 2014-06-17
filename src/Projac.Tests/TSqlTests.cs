using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests
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
        public void NonQueryFormatReturnsExpectedInstance(TSqlNonQueryStatement actual, TSqlNonQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryFormatCases()
        {
            yield return new TestCaseData(
                TSql.NonQueryFormat("text"),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", parameters: null),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new ITSqlParameterValue[0]),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestSqlParameter()),
                new TSqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0}", new TestSqlParameter()),
                new TSqlNonQueryStatement("text @P0", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text", new TestSqlParameter(), new TestSqlParameter()),
                new TSqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0"),
                    new TestSqlParameter().ToSqlParameter("@P1")
                }));
            yield return new TestCaseData(
                TSql.NonQueryFormat("text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new TSqlNonQueryStatement("text @P0 @P1", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0"),
                    new TestSqlParameter().ToSqlParameter("@P1")
                }));
        }

        [TestCaseSource("QueryFormatCases")]
        public void QueryFormatReturnsExpectedInstance(TSqlQueryStatement actual, TSqlQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryFormatCases()
        {
            yield return new TestCaseData(
               TSql.QueryFormat("text"),
               new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", parameters: null),
                new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new ITSqlParameterValue[0]),
                new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestSqlParameter()),
                new TSqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0}", new TestSqlParameter()),
                new TSqlQueryStatement("text @P0", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text", new TestSqlParameter(), new TestSqlParameter()),
                new TSqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0"),
                    new TestSqlParameter().ToSqlParameter("@P1")
                }));
            yield return new TestCaseData(
                TSql.QueryFormat("text {0} {1}", new TestSqlParameter(), new TestSqlParameter()),
                new TSqlQueryStatement("text @P0 @P1", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@P0"),
                    new TestSqlParameter().ToSqlParameter("@P1")
                }));
        }

        [Test]
        public void ComposeStatementArrayCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((TSqlNonQueryStatement[])null));
        }

        [Test]
        public void ComposeStatementEnumerationCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => TSql.Compose((IEnumerable<TSqlNonQueryStatement>)null));
        }

        [Test]
        public void ComposeStatementArrayReturnsComposer()
        {
            Assert.IsInstanceOf<TSqlNonQueryStatementComposer>(
                TSql.Compose(
                    StatementFactory(),
                    StatementFactory()));
        }
        [Test]
        public void ComposeStatementEnumerationReturnsComposer()
        {
            Assert.IsInstanceOf<TSqlNonQueryStatementComposer>(
                TSql.Compose((IEnumerable<TSqlNonQueryStatement>)new[]
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

            TSqlNonQueryStatement[] result = TSql.Compose(statement1, statement2);

            Assert.That(result, Is.EquivalentTo(new []
            {
                statement1, statement2
            }));
        }

        [Test]
        public void ComposedStatementEnumerationIsPreservedAndReturnedByComposer()
        {
            var statement1 = StatementFactory();
            var statement2 = StatementFactory();

            TSqlNonQueryStatement[] result = TSql.Compose((IEnumerable<TSqlNonQueryStatement>)new[]
            {
                statement1, statement2
            });

            Assert.That(result, Is.EquivalentTo(new[]
            {
                statement1, statement2
            }));
        }

        private static TSqlNonQueryStatement StatementFactory()
        {
            return new TSqlNonQueryStatement("text", new SqlParameter[0]);
        }

        [Test]
        public void ProjectionReturnsBuilder()
        {
            var result = TSql.Projection();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<TSqlProjectionBuilder>());
        }

        [Test]
        public void ProjectionReturnsNewBuilderUponEachCall()
        {
            var result1 = TSql.Projection();
            var result2 = TSql.Projection();

            Assert.That(result1, Is.Not.SameAs(result2));
        }

        class TestSqlParameter : ITSqlParameterValue
        {
            public SqlParameter ToSqlParameter(string parameterName)
            {
                return new SqlParameter(parameterName, DBNull.Value);
            }
        }
    }
}
