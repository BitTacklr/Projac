#if !FSHARP
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Projac.Tests.Framework;
using NUnit.Framework;

namespace Projac.Tests
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
        public void QueryReturnsExpectedInstance(TSqlQueryStatement actual, TSqlQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> QueryCases()
        {
            yield return new TestCaseData(
                TSql.Query("text"),
                new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", parameters: null),
                new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { }),
                new TSqlQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter = new TestSqlParameter() }),
                new TSqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.Query("text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new TSqlQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@Parameter1"),
                    new TestSqlParameter().ToSqlParameter("@Parameter2")
                }));
        }

        [TestCaseSource("NonQueryCases")]
        public void NonQueryReturnsExpectedInstance(TSqlNonQueryStatement actual, TSqlNonQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private static IEnumerable<TestCaseData> NonQueryCases()
        {
            yield return new TestCaseData(
                TSql.NonQuery("text"),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", parameters: null),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { }),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter = new TestSqlParameter() }),
                new TSqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.NonQuery("text", new { Parameter1 = new TestSqlParameter(), Parameter2 = new TestSqlParameter() }),
                new TSqlNonQueryStatement("text", new[]
                {
                    new TestSqlParameter().ToSqlParameter("@Parameter1"),
                    new TestSqlParameter().ToSqlParameter("@Parameter2")
                }));
        }
    }
}
#endif