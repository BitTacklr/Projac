using System;
using System.Data;
using NUnit.Framework;
using Paramol.SQLite;

namespace Paramol.Tests.SQLite
{
    public partial class SQLiteSyntaxTests
    {
        [Test]
        public void AnsiStringReturnsExpectedInstance()
        {
            Assert.That(Sql.AnsiString("value"), Is.EqualTo(new SQLiteDbParameterValue(DbType.AnsiString, "value")));
        }

        [Test]
        public void AnsiStringNullReturnsExpectedInstance()
        {
            Assert.That(Sql.AnsiString(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.AnsiString, DBNull.Value)));
        }

        [Test]
        public void AnsiStringFixedLengthReturnsExpectedInstance()
        {
            Assert.That(Sql.AnsiStringFixedLength("value"), Is.EqualTo(new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, "value")));
        }

        [Test]
        public void AnsiStringFixedLengthNullReturnsExpectedInstance()
        {
            Assert.That(Sql.AnsiStringFixedLength(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, DBNull.Value)));
        }

        [Test]
        public void BinaryReturnsExpectedInstance()
        {
            var value = new byte[] { 1,2,3,4};
            Assert.That(Sql.Binary(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Binary, value)));
        }

        [Test]
        public void BinaryNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Binary(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Binary, DBNull.Value)));
        }

        [Test]
        public void BooleanReturnsExpectedInstance()
        {
            Assert.That(Sql.Boolean(true), Is.EqualTo(new SQLiteDbParameterValue(DbType.Boolean, true)));
        }

        [Test]
        public void BooleanNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Boolean(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Boolean, DBNull.Value)));
        }

        [Test]
        public void ByteReturnsExpectedInstance()
        {
            byte value = 123;
            Assert.That(Sql.Byte(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Byte, value)));
        }

        [Test]
        public void ByteNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Byte(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Byte, DBNull.Value)));
        }

        [Test]
        public void CurrencyReturnsExpectedInstance()
        {
            double value = 123.00;
            Assert.That(Sql.Currency(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Currency, value)));
        }

        [Test]
        public void CurrencyNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Currency(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Currency, DBNull.Value)));
        }

        [Test]
        public void DateReturnsExpectedInstance()
        {
            var value = DateTime.Now;
            Assert.That(Sql.Date(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Date, value)));
        }

        [Test]
        public void DateNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Date(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Date, DBNull.Value)));
        }

        [Test]
        public void DateTimeReturnsExpectedInstance()
        {
            var value = DateTime.Now;
            Assert.That(Sql.DateTime(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.DateTime, value)));
        }

        [Test]
        public void DateTimeNullReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTime(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.DateTime, DBNull.Value)));
        }

        [Test]
        public void DateTime2ReturnsExpectedInstance()
        {
            var value = DateTime.Now;
            Assert.That(Sql.DateTime2(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.DateTime2, value)));
        }

        [Test]
        public void DateTime2NullReturnsExpectedInstance()
        {
            Assert.That(Sql.DateTime2(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.DateTime2, DBNull.Value)));
        }

        [Test]
        public void DecimalReturnsExpectedInstance()
        {
            decimal value = 123;
            Assert.That(Sql.Decimal(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Decimal, value)));
        }

        [Test]
        public void DecimalNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Decimal(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Decimal, DBNull.Value)));
        }

        [Test]
        public void DoubleReturnsExpectedInstance()
        {
            double value = 123.00;
            Assert.That(Sql.Double(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Double, value)));
        }

        [Test]
        public void DoubleNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Double(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Double, DBNull.Value)));
        }

        [Test]
        public void GuidReturnsExpectedInstance()
        {
            var value = Guid.NewGuid();
            Assert.That(Sql.Guid(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Guid, value)));
        }

        [Test]
        public void GuidNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Guid(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Guid, DBNull.Value)));
        }

        [Test]
        public void Int16ReturnsExpectedInstance()
        {
            short value = 5;
            Assert.That(Sql.Int16(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int16, value)));
        }

        [Test]
        public void Int16NullReturnsExpectedInstance()
        {
            Assert.That(Sql.Int16(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int16, DBNull.Value)));
        }

        [Test]
        public void Int32ReturnsExpectedInstance()
        {
            int value = 5;
            Assert.That(Sql.Int32(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int32, value)));
        }

        [Test]
        public void Int32NullReturnsExpectedInstance()
        {
            Assert.That(Sql.Int32(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int32, DBNull.Value)));
        }

        [Test]
        public void Int64ReturnsExpectedInstance()
        {
            long value = 5;
            Assert.That(Sql.Int64(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int64, value)));
        }

        [Test]
        public void Int64NullReturnsExpectedInstance()
        {
            Assert.That(Sql.Int64(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Int64, DBNull.Value)));
        }

        [Test]
        public void SByteReturnsExpectedInstance()
        {
            sbyte value = 5;
            Assert.That(Sql.SByte(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.SByte, value)));
        }

        [Test]
        public void SByteNullReturnsExpectedInstance()
        {
            Assert.That(Sql.SByte(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.SByte, DBNull.Value)));
        }

        [Test]
        public void SingleReturnsExpectedInstance()
        {
            float value = 5;
            Assert.That(Sql.Single(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Single, value)));
        }

        [Test]
        public void SingleNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Single(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Single, DBNull.Value)));
        }

        [Test]
        public void StringReturnsExpectedInstance()
        {
            Assert.That(Sql.String("value"), Is.EqualTo(new SQLiteDbParameterValue(DbType.String, "value")));
        }

        [Test]
        public void StringNullReturnsExpectedInstance()
        {
            Assert.That(Sql.String(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.String, DBNull.Value)));
        }

        [Test]
        public void StringFixedLengthReturnsExpectedInstance()
        {
            Assert.That(Sql.StringFixedLength("value"), Is.EqualTo(new SQLiteDbParameterValue(DbType.StringFixedLength, "value")));
        }

        [Test]
        public void StringFixedLengthNullReturnsExpectedInstance()
        {
            Assert.That(Sql.StringFixedLength(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.StringFixedLength, DBNull.Value)));
        }

        [Test]
        public void TimeReturnsExpectedInstance()
        {
            var value = TimeSpan.FromTicks(100);
            Assert.That(Sql.Time(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.Time, value)));
        }

        [Test]
        public void TimeNullReturnsExpectedInstance()
        {
            Assert.That(Sql.Time(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.Time, DBNull.Value)));
        }

        [Test]
        public void UInt16ReturnsExpectedInstance()
        {
            ushort value = 5;
            Assert.That(Sql.UInt16(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt16, value)));
        }

        [Test]
        public void UInt16NullReturnsExpectedInstance()
        {
            Assert.That(Sql.UInt16(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt16, DBNull.Value)));
        }

        [Test]
        public void UInt32ReturnsExpectedInstance()
        {
            uint value = 5;
            Assert.That(Sql.UInt32(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt32, value)));
        }

        [Test]
        public void UInt32NullReturnsExpectedInstance()
        {
            Assert.That(Sql.UInt32(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt32, DBNull.Value)));
        }

        [Test]
        public void UInt64ReturnsExpectedInstance()
        {
            ulong value = 5;
            Assert.That(Sql.UInt64(value), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt64, value)));
        }

        [Test]
        public void UInt64NullReturnsExpectedInstance()
        {
            Assert.That(Sql.UInt64(null), Is.EqualTo(new SQLiteDbParameterValue(DbType.UInt64, DBNull.Value)));
        }
    }
}