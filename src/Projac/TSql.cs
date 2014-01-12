using System;

namespace Projac
{
    /// <summary>
    /// Fluent T-SQL syntax.
    /// </summary>
    public static class TSql
    {
        /// <summary>
        /// Returns a NULL parameter value.
        /// </summary>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue Null()
        {
            return TSqlNullValue.Instance;
        }

        /// <summary>
        /// Returns a VARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue VarChar(string value, TSqlVarCharSize size)
        {
            return value == null ? 
                Null() : 
                new TSqlVarCharValue(value, size);
        }

        /// <summary>
        /// Returns a VARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue VarCharMax(string value)
        {
            return value == null ? 
                Null() : 
                new TSqlVarCharValue(value, TSqlVarCharSize.Max);
        }

        /// <summary>
        /// Returns a NVARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue NVarChar(string value, TSqlNVarCharSize size)
        {
            return value == null ?
                Null() : 
                new TSqlNVarCharValue(value, size);
        }

        /// <summary>
        /// Returns a NVARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue NVarCharMax(string value)
        {
            return value == null ?
                Null() : 
                new TSqlNVarCharValue(value, TSqlNVarCharSize.Max);
        }

        /// <summary>
        /// Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue BigInt(long? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        /// Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue Int(int? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlIntValue(value.Value);
        }

        /// <summary>
        /// Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue Bit(bool? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlBitValue(value.Value);
        }

        /// <summary>
        /// Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue"/>.</returns>
        public static ITSqlParameterValue UniqueIdentifier(Guid? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlUniqueIdentifierValue(value.Value);
        }
    }
}