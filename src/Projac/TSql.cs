namespace Projac
{
    /// <summary>
    /// Fluent T-SQL syntax.
    /// </summary>
    public static class TSql
    {
        /// <summary>
        /// Returns the NULL parameter value.
        /// </summary>
        /// <returns>A <see cref="TSqlNullValue"/> instance.</returns>
        public static ITSqlParameterValue Null()
        {
            return TSqlNullValue.Instance;
        }

        /// <summary>
        /// Returns the VARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="TSqlVarCharValue"/>.</returns>
        public static ITSqlParameterValue VarChar(string value, TSqlVarCharSize size)
        {
            return new TSqlVarCharValue(value, size);
        }

        /// <summary>
        /// Returns the VARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="TSqlVarCharValue"/>.</returns>
        public static ITSqlParameterValue VarCharMax(string value)
        {
            return new TSqlVarCharValue(value, TSqlVarCharSize.Max);
        }

        /// <summary>
        /// Returns the NVARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="TSqlNVarCharValue"/>.</returns>
        public static ITSqlParameterValue NVarChar(string value, TSqlNVarCharSize size)
        {
            return new TSqlNVarCharValue(value, size);
        }

        /// <summary>
        /// Returns the NVARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="TSqlNVarCharValue"/>.</returns>
        public static ITSqlParameterValue NVarCharMax(string value)
        {
            return new TSqlNVarCharValue(value, TSqlNVarCharSize.Max);
        }
    }
}