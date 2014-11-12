using System;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
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
            Assert.That(TSql.Binary(new byte[] { 1, 2, 3 }, 123), Is.EqualTo(new TSqlBinaryValue(new byte[] { 1, 2, 3 }, new TSqlBinarySize(123))));
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
        public void DateReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(TSql.Date(value), Is.EqualTo(new TSqlDateValue(value)));
        }

        [Test]
        public void DateNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Date(null), Is.EqualTo(TSqlDateNullValue.Instance));
        }

        [Test]
        public void DateTimeReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(TSql.DateTime(value), Is.EqualTo(new TSqlDateTimeValue(value)));
        }

        [Test]
        public void DateTimeNullReturnsExpectedInstance()
        {
            Assert.That(TSql.DateTime(null), Is.EqualTo(TSqlDateTimeNullValue.Instance));
        }

        [Test]
        public void DateTime2ReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(TSql.DateTime2(value, 3), Is.EqualTo(new TSqlDateTime2Value(value, 3)));
        }

        [Test]
        public void DateTime2NullReturnsExpectedInstance()
        {
            Assert.That(TSql.DateTime2(null, 3), Is.EqualTo(new TSqlDateTime2NullValue(new TSqlDateTime2Precision(3))));
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

        [Test]
        public void MoneyReturnsExpectedInstance()
        {
            Assert.That(TSql.Money(123.45M), Is.EqualTo(new TSqlMoneyValue(123.45M)));
        }

        [Test]
        public void MoneyNullReturnsExpectedInstance()
        {
            Assert.That(TSql.Money(null), Is.EqualTo(TSqlMoneyNullValue.Instance));
        }
    }
}