using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class TSqlTests
    {
        [Test]
        public void NullReturnsSqlNullValueInstance()
        {
            Assert.That(TSql.Null(), Is.SameAs(TSqlNullValue.Instance));
        }

        [Test]
        public void VarCharReturnsExpectedInstance()
        {
            Assert.That(TSql.VarChar("value", 123), Is.EqualTo(new TSqlVarCharValue("value", new TSqlVarCharSize(123))));
        }

        [Test]
        public void VarCharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarChar(null, 123), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void VarCharMaxReturnsExpectedInstance()
        {
            Assert.That(TSql.VarCharMax("value"), Is.EqualTo(new TSqlVarCharValue("value", TSqlVarCharSize.Max)));
        }

        [Test]
        public void VarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(TSql.VarCharMax(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void NVarCharReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarChar("value", 123), Is.EqualTo(new TSqlNVarCharValue("value", new TSqlNVarCharSize(123))));
        }

        [Test]
        public void NVarCharNullReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarChar(null, 123), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void NVarCharMaxReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarCharMax("value"), Is.EqualTo(new TSqlNVarCharValue("value", TSqlNVarCharSize.Max)));
        }

        [Test]
        public void NVarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(TSql.NVarCharMax(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void BigIntReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(123), Is.EqualTo(new TSqlBigIntValue(123)));
        }

        [Test]
        public void BigIntNullReturnsExpectedInstance()
        {
            Assert.That(TSql.BigInt(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void IntReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(123), Is.EqualTo(new TSqlIntValue(123)));
        }

        [Test]
        public void IntNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Int(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void BitReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(true), Is.EqualTo(new TSqlBitValue(true)));
        }

        [Test]
        public void BitNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Bit(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [Test]
        public void UniqueIdentifierReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(Guid.Empty), Is.EqualTo(new TSqlUniqueIdentifierValue(Guid.Empty)));
        }

        [Test]
        public void UniqueIdentifierNullReturnsExpectedInstance()
        {
            Assert.That(TSql.UniqueIdentifier(null), Is.EqualTo(TSqlNullValue.Instance));
        }

        [TestCaseSource("StatementCases")]
        public void StatementReturnsExpectedInstance(TSqlNonQueryStatement actual, TSqlNonQueryStatement expected)
        {
            Assert.That(actual.Text, Is.EqualTo(expected.Text));
            Assert.That(actual.Parameters, Is.EquivalentTo(expected.Parameters).Using(new SqlParameterEqualityComparer()));
        }

        private IEnumerable<TestCaseData> StatementCases()
        {
            yield return new TestCaseData(
                TSql.Statement("text"),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Statement("text", parameters: null),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Statement("text", new { }),
                new TSqlNonQueryStatement("text", new SqlParameter[0]));
            yield return new TestCaseData(
                TSql.Statement("text", new { Parameter = TSql.Bit(true) }),
                new TSqlNonQueryStatement("text", new []
                {
                    new TSqlBitValue(true).ToSqlParameter("@Parameter")
                }));
            yield return new TestCaseData(
                TSql.Statement("text", new { Parameter1 = TSql.Bit(true), Parameter2 = TSql.Bit(null) }),
                new TSqlNonQueryStatement("text", new[]
                {
                    new TSqlBitValue(true).ToSqlParameter("@Parameter1"),
                    TSqlNullValue.Instance.ToSqlParameter("@Parameter2")
                }));
        }
    }
}
