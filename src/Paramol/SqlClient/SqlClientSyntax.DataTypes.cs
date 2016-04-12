using System;

namespace Paramol.SqlClient
{
    public partial class SqlClientSyntax
    {
        /// <summary>
        ///     Returns a VARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue VarChar(string value, TSqlVarCharSize size)
        {
            if (value == null)
                return new TSqlVarCharNullValue(size);
            return new TSqlVarCharValue(value, size);
        }

        /// <summary>
        ///     Returns a CHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Char(string value, TSqlCharSize size)
        {
            if (value == null)
                return new TSqlCharNullValue(size);
            return new TSqlCharValue(value, size);
        }

        /// <summary>
        ///     Returns a VARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue VarCharMax(string value)
        {
            if (value == null)
                return new TSqlVarCharNullValue(TSqlVarCharSize.Max);
            return new TSqlVarCharValue(value, TSqlVarCharSize.Max);
        }

        /// <summary>
        ///     Returns a NVARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue NVarChar(string value, TSqlNVarCharSize size)
        {
            if (value == null)
                return new TSqlNVarCharNullValue(size);
            return new TSqlNVarCharValue(value, size);
        }

        /// <summary>
        ///     Returns a NCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue NChar(string value, TSqlNCharSize size)
        {
            if (value == null)
                return new TSqlNCharNullValue(size);
            return new TSqlNCharValue(value, size);
        }

        /// <summary>
        ///     Returns a NVARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue NVarCharMax(string value)
        {
            if (value == null)
                return new TSqlNVarCharNullValue(TSqlNVarCharSize.Max);
            return new TSqlNVarCharValue(value, TSqlNVarCharSize.Max);
        }

        /// <summary>
        ///     Returns a BINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Binary(byte[] value, TSqlBinarySize size)
        {
            if (value == null)
                return new TSqlBinaryNullValue(size);
            return new TSqlBinaryValue(value, size);
        }

        /// <summary>
        ///     Returns a VARBINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue VarBinary(byte[] value, TSqlVarBinarySize size)
        {
            if (value == null)
                return new TSqlVarBinaryNullValue(size);
            return new TSqlVarBinaryValue(value, size);
        }

        /// <summary>
        ///     Returns a VARBINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue VarBinaryMax(byte[] value)
        {
            if (value == null)
                return new TSqlVarBinaryNullValue(TSqlVarBinarySize.Max);
            return new TSqlVarBinaryValue(value, TSqlVarBinarySize.Max);
        }

        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue BigInt(long? value)
        {
            if (!value.HasValue)
                return TSqlBigIntNullValue.Instance;
            return new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Int(int? value)
        {
            if (!value.HasValue)
                return TSqlIntNullValue.Instance;
            return new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Bit(bool? value)
        {
            if (!value.HasValue)
                return TSqlBitNullValue.Instance;
            return new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue UniqueIdentifier(Guid? value)
        {
            if (!value.HasValue)
                return TSqlUniqueIdentifierNullValue.Instance;
            return new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATE parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Date(DateTime? value)
        {
            if (value == null)
                return TSqlDateNullValue.Instance;
            return new TSqlDateValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIME parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTime(DateTime? value)
        {
            if (!value.HasValue)
                return TSqlDateTimeNullValue.Instance;
            return new TSqlDateTimeValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIME2 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTime2(DateTime? value)
        {
            return DateTime2(value, TSqlDateTime2Precision.Default);
        }

        /// <summary>
        ///     Returns a DATETIME2 parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="precision">The parameter precision.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTime2(DateTime? value, TSqlDateTime2Precision precision)
        {
            if (!value.HasValue)
                return new TSqlDateTime2NullValue(precision);
            return new TSqlDateTime2Value(value.Value, precision);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue DateTimeOffset(DateTimeOffset? value)
        {
            if (!value.HasValue)
                return TSqlDateTimeOffsetNullValue.Instance;
            return new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a DECIMAL parameter value.
        /// </summary>
        /// <param name="value">The parameter value</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Decimal(decimal? value)
        {
            return Decimal(value, TSqlDecimalPrecision.Default, TSqlDecimalScale.Default);
        }

        /// <summary>
        ///     Returns a DECIMAL parameter value.
        /// </summary>
        /// <param name="value">The parameter value</param>
        /// <param name="precision">The parameter precision.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Decimal(decimal? value, TSqlDecimalPrecision precision)
        {
            return Decimal(value, precision, TSqlDecimalScale.Default);
        }

        /// <summary>
        ///     Returns a DECIMAL parameter value.
        /// </summary>
        /// <param name="value">The parameter value</param>
        /// <param name="precision">The parameter precision.</param>
        /// <param name="scale">The parameter scale.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Decimal(decimal? value, TSqlDecimalPrecision precision, TSqlDecimalScale scale)
        {
            if (!value.HasValue)
                return new TSqlDecimalNullValue(precision, scale);
            return new TSqlDecimalValue(value.Value, precision, scale);
        }

        /// <summary>
        ///     Returns a MONEY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public IDbParameterValue Money(decimal? value)
        {
            if (!value.HasValue)
                return TSqlMoneyNullValue.Instance;
            return new TSqlMoneyValue(value.Value);
        }
    }
}