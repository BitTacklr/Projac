using System;

namespace Paramol.SQLite
{
    public partial class SQLiteSyntax
    {
        /// <summary>
        ///     Returns a AnsiString parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue AnsiString(string value)
        {
            if (value == null)
                return SQLiteDbParameterValue.AnsiStringNull;
            return SQLiteDbParameterValue.AnsiStringFactory(value);
        }

        /// <summary>
        ///     Returns a AnsiStringFixedLength parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue AnsiStringFixedLength(string value)
        {
            if (value == null)
                return SQLiteDbParameterValue.AnsiStringFixedLengthNull;
            return SQLiteDbParameterValue.AnsiStringFixedLengthFactory(value);
        }

        /// <summary>
        ///     Returns a Binary parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Binary(byte[] value)
        {
            if (value == null)
                return SQLiteDbParameterValue.BinaryNull;
            return SQLiteDbParameterValue.BinaryFactory(value);
        }

        /// <summary>
        ///     Returns a Boolean parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Boolean(bool? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.BooleanNull;
            return SQLiteDbParameterValue.BooleanFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Byte parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Byte(byte? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.ByteNull;
            return SQLiteDbParameterValue.ByteFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Currency parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Currency(double? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.CurrencyNull;
            return SQLiteDbParameterValue.CurrencyFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Date parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Date(DateTime? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DateNull;
            return SQLiteDbParameterValue.DateFactory(value.Value);
        }

        /// <summary>
        ///     Returns a DateTime parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTime(DateTime? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DateTimeNull;
            return SQLiteDbParameterValue.DateTimeFactory(value.Value);
        }

        /// <summary>
        ///     Returns a DateTime2 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTime2(DateTime? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DateTime2Null;
            return SQLiteDbParameterValue.DateTime2Factory(value.Value);
        }

        /// <summary>
        ///     Returns a DateTimeOffset parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTimeOffset(DateTimeOffset? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DateTimeOffsetNull;
            return SQLiteDbParameterValue.DateTimeOffsetFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Decimal parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Decimal(decimal? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DecimalNull;
            return SQLiteDbParameterValue.DecimalFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Double parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Double(double? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.DoubleNull;
            return SQLiteDbParameterValue.DoubleFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Guid parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Guid(Guid? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.GuidNull;
            return SQLiteDbParameterValue.GuidFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Int16 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Int16(short? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.Int16Null;
            return SQLiteDbParameterValue.Int16Factory(value.Value);
        }

        /// <summary>
        ///     Returns a Int32 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Int32(int? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.Int32Null;
            return SQLiteDbParameterValue.Int32Factory(value.Value);
        }

        /// <summary>
        ///     Returns a Int64 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Int64(long? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.Int64Null;
            return SQLiteDbParameterValue.Int64Factory(value.Value);
        }

        /// <summary>
        ///     Returns a SByte parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue SByte(sbyte? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.SByteNull;
            return SQLiteDbParameterValue.SByteFactory(value.Value);
        }

        /// <summary>
        ///     Returns a Single parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Single(float? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.SingleNull;
            return SQLiteDbParameterValue.SingleFactory(value.Value);
        }
        
        /// <summary>
        ///     Returns a String parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue String(string value)
        {
            if (value == null)
                return SQLiteDbParameterValue.StringNull;
            return SQLiteDbParameterValue.StringFactory(value);
        }

        /// <summary>
        ///     Returns a StringFixedLength parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue StringFixedLength(string value)
        {
            if (value == null)
                return SQLiteDbParameterValue.StringFixedLengthNull;
            return SQLiteDbParameterValue.StringFixedLengthFactory(value);
        }

        /// <summary>
        ///     Returns a Time parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Time(TimeSpan? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.TimeNull;
            return SQLiteDbParameterValue.TimeFactory(value);
        }

        /// <summary>
        ///     Returns a UInt16 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue UInt16(ushort? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.UInt16Null;
            return SQLiteDbParameterValue.UInt16Factory(value.Value);
        }

        /// <summary>
        ///     Returns a UInt32 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue UInt32(uint? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.UInt32Null;
            return SQLiteDbParameterValue.UInt32Factory(value.Value);
        }

        /// <summary>
        ///     Returns a UInt64 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue UInt64(ulong? value)
        {
            if (!value.HasValue)
                return SQLiteDbParameterValue.UInt64Null;
            return SQLiteDbParameterValue.UInt64Factory(value.Value);
        }

        //TODO: Object, VarNumeric, Xml.
    }
}
