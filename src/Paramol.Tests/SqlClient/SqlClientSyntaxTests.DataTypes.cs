using System;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SqlClient
{
    public partial class SqlClientSyntaxTests
    {
        [Test]
        public void VarCharReturnsExpectedInstance()
        {
            Assert.That(Sql.VarChar("value", 123), Is.EqualTo(new TSqlVarCharValue("value", new TSqlVarCharSize(123))));
        }

        [Test]
        public void VarCharNullReturnsExpectedInstance()
        {
            Assert.That(Sql.VarChar(null, 123), Is.EqualTo(new TSqlVarCharNullValue(new TSqlVarCharSize(123))));
        }

        [Test]
        public void CharReturnsExpectedInstance()
        {
            Assert.That(Sql.Char("value", 123), Is.EqualTo(new TSqlCharValue("value", new TSqlCharSize(123))));
        }

        [Test]
        public void CharNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Char(null, 123), Is.EqualTo(new TSqlCharNullValue(new TSqlCharSize(123))));
        }

        [Test]
        public void VarCharMaxReturnsExpectedInstance()
        {
            Assert.That(Sql.VarCharMax("value"), Is.EqualTo(new TSqlVarCharValue("value", TSqlVarCharSize.Max)));
        }

        [Test]
        public void VarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(Sql.VarCharMax(null), Is.EqualTo(new TSqlVarCharNullValue(TSqlVarCharSize.Max)));
        }

        [Test]
        public void NVarCharReturnsExpectedInstance()
        {
            Assert.That(Sql.NVarChar("value", 123), Is.EqualTo(new TSqlNVarCharValue("value", new TSqlNVarCharSize(123))));
        }

        [Test]
        public void NVarCharNullReturnsExpectedInstance()
        {
            Assert.That(Sql.NVarChar(null, 123), Is.EqualTo(new TSqlNVarCharNullValue(new TSqlNVarCharSize(123))));
        }

        [Test]
        public void NCharReturnsExpectedInstance()
        {
            Assert.That(Sql.NChar("value", 123), Is.EqualTo(new TSqlNCharValue("value", new TSqlNCharSize(123))));
        }

        [Test]
        public void NCharNullReturnsExpectedInstance()
        {
            Assert.That(Sql.NChar(null, 123), Is.EqualTo(new TSqlNCharNullValue(new TSqlNCharSize(123))));
        }

        [Test]
        public void NVarCharMaxReturnsExpectedInstance()
        {
            Assert.That(Sql.NVarCharMax("value"), Is.EqualTo(new TSqlNVarCharValue("value", TSqlNVarCharSize.Max)));
        }

        [Test]
        public void NVarCharMaxNullReturnsExpectedInstance()
        {
            Assert.That(Sql.NVarCharMax(null), Is.EqualTo(new TSqlNVarCharNullValue(TSqlNVarCharSize.Max)));
        }

        [Test]
        public void BinaryReturnsExpectedInstance()
        {
            Assert.That(Sql.Binary(new byte[] { 1, 2, 3 }, 123), Is.EqualTo(new TSqlBinaryValue(new byte[] { 1, 2, 3 }, new TSqlBinarySize(123))));
        }

        [Test]
        public void BinaryNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Binary(null, 123), Is.EqualTo(new TSqlBinaryNullValue(new TSqlBinarySize(123))));
        }

        [Test]
        public void VarBinaryReturnsExpectedInstance()
        {
            Assert.That(Sql.VarBinary(new byte[] { 1, 2, 3 }, 123), Is.EqualTo(new TSqlVarBinaryValue(new byte[] { 1, 2, 3 }, new TSqlVarBinarySize(123))));
        }

        [Test]
        public void VarBinaryNullReturnsExpectedInstance()
        {
            Assert.That(Sql.VarBinary(null, 123), Is.EqualTo(new TSqlVarBinaryNullValue(new TSqlVarBinarySize(123))));
        }

        [Test]
        public void VarBinaryMaxReturnsExpectedInstance()
        {
            Assert.That(Sql.VarBinaryMax(new byte[] { 1, 2, 3 }), Is.EqualTo(new TSqlVarBinaryValue(new byte[] { 1, 2, 3 }, TSqlVarBinarySize.Max)));
        }

        [Test]
        public void VarBinaryMaxNullReturnsExpectedInstance()
        {
            Assert.That(Sql.VarBinaryMax(null), Is.EqualTo(new TSqlVarBinaryNullValue(TSqlVarBinarySize.Max)));
        }

        [Test]
        public void BigIntReturnsExpectedInstance()
        {
            Assert.That(Sql.BigInt(123), Is.EqualTo(new TSqlBigIntValue(123)));
        }

        [Test]
        public void BigIntNullReturnsExpectedInstance()
        {
            Assert.That(Sql.BigInt(null), Is.EqualTo(TSqlBigIntNullValue.Instance));
        }

        [Test]
        public void IntReturnsExpectedInstance()
        {
            Assert.That(Sql.Int(123), Is.EqualTo(new TSqlIntValue(123)));
        }

        [Test]
        public void IntNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Int(null), Is.EqualTo(TSqlIntNullValue.Instance));
        }

        [Test]
        public void BitReturnsExpectedInstance()
        {
            Assert.That(Sql.Bit(true), Is.EqualTo(new TSqlBitValue(true)));
        }

        [Test]
        public void BitNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Bit(null), Is.EqualTo(TSqlBitNullValue.Instance));
        }

        [Test]
        public void UniqueIdentifierReturnsExpectedInstance()
        {
            Assert.That(Sql.UniqueIdentifier(Guid.Empty), Is.EqualTo(new TSqlUniqueIdentifierValue(Guid.Empty)));
        }

        [Test]
        public void UniqueIdentifierNullReturnsExpectedInstance()
        {
            Assert.That(Sql.UniqueIdentifier(null), Is.EqualTo(TSqlUniqueIdentifierNullValue.Instance));
        }

        [Test]
        public void DateReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(Sql.Date(value), Is.EqualTo(new TSqlDateValue(value)));
        }

        [Test]
        public void DateNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Date(null), Is.EqualTo(TSqlDateNullValue.Instance));
        }

        [Test]
        public void DateTimeReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(Sql.DateTime(value), Is.EqualTo(new TSqlDateTimeValue(value)));
        }

        [Test]
        public void DateTimeNullReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTime(null), Is.EqualTo(TSqlDateTimeNullValue.Instance));
        }

        [Test]
        public void DateTime2WithoutPrecisionReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(Sql.DateTime2(value), Is.EqualTo(new TSqlDateTime2Value(value, 7)));
        }

        [Test]
        public void DateTime2ReturnsExpectedInstance()
        {
            var value = DateTime.UtcNow;
            Assert.That(Sql.DateTime2(value, 3), Is.EqualTo(new TSqlDateTime2Value(value, 3)));
        }

        [Test]
        public void DateTime2NullReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTime2(null, 3), Is.EqualTo(new TSqlDateTime2NullValue(new TSqlDateTime2Precision(3))));
        }

        [Test]
        public void DateTime2NullWithoutPrecisionReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTime2(null), Is.EqualTo(new TSqlDateTime2NullValue(TSqlDateTime2Precision.Default)));
        }

        [Test]
        public void DateTimeOffsetReturnsExpectedInstance()
        {
            var value = DateTimeOffset.UtcNow;
            Assert.That(Sql.DateTimeOffset(value), Is.EqualTo(new TSqlDateTimeOffsetValue(value)));
        }

        [Test]
        public void DateTimeOffsetNullReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTimeOffset(null), Is.EqualTo(TSqlDateTimeOffsetNullValue.Instance));
        }

        [Test]
        public void MoneyReturnsExpectedInstance()
        {
            Assert.That(Sql.Money(123.45M), Is.EqualTo(new TSqlMoneyValue(123.45M)));
        }

        [Test]
        public void MoneyNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Money(null), Is.EqualTo(TSqlMoneyNullValue.Instance));
        }

        [Test]
        public void DecimalWithoutPrecisionNorScaleReturnsExpectedInstance()
        {
            var value = 5;
            Assert.That(Sql.Decimal(value), Is.EqualTo(new TSqlDecimalValue(value, 18, 0)));
        }

        [Test]
        public void DecimalWithoutScaleReturnsExpectedInstance()
        {
            var value = 5;
            Assert.That(Sql.Decimal(value, TSqlDecimalPrecision.Max), Is.EqualTo(new TSqlDecimalValue(value, TSqlDecimalPrecision.Max, TSqlDecimalScale.Default)));
        }

        [Test]
        public void DecimalReturnsExpectedInstance()
        {
            var value = 5;
            Assert.That(Sql.Decimal(value, 3, 1), Is.EqualTo(new TSqlDecimalValue(value, 3, 1)));
        }

        [Test]
        public void DecimalNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Decimal(null, 3, 1), Is.EqualTo(new TSqlDecimalNullValue(3, 1)));
        }

        [Test]
        public void DecimalNullWithoutScaleReturnsExpectedInstance()
        {
            Assert.That(Sql.Decimal(null, TSqlDecimalPrecision.Max), Is.EqualTo(new TSqlDecimalNullValue(TSqlDecimalPrecision.Max, TSqlDecimalScale.Default)));
        }

        [Test]
        public void DecimalNullWithoutPrecisionNorScaleReturnsExpectedInstance()
        {
            Assert.That(Sql.Decimal(null), Is.EqualTo(new TSqlDecimalNullValue(TSqlDecimalPrecision.Default, TSqlDecimalScale.Default)));
        }
    }
}
