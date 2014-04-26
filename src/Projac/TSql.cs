using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Projac
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    public static class TSql
    {
        /// <summary>
        ///     Returns a NULL parameter value.
        /// </summary>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Null()
        {
            return TSqlNullValue.Instance;
        }

        /// <summary>
        ///     Returns a VARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue VarChar(string value, TSqlVarCharSize size)
        {
            return value == null
                ? Null()
                : new TSqlVarCharValue(value, size);
        }

        /// <summary>
        ///     Returns a CHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Char(string value, TSqlCharSize size)
        {
            return value == null
                ? Null()
                : new TSqlCharValue(value, size);
        }

        /// <summary>
        ///     Returns a VARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue VarCharMax(string value)
        {
            return value == null
                ? Null()
                : new TSqlVarCharValue(value, TSqlVarCharSize.Max);
        }

        /// <summary>
        ///     Returns a NVARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue NVarChar(string value, TSqlNVarCharSize size)
        {
            return value == null
                ? Null()
                : new TSqlNVarCharValue(value, size);
        }

        /// <summary>
        ///     Returns a NCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue NChar(string value, TSqlNCharSize size)
        {
            return value == null
                ? Null()
                : new TSqlNCharValue(value, size);
        }

        /// <summary>
        ///     Returns a NVARCHAR(MAX) parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue NVarCharMax(string value)
        {
            return value == null
                ? Null()
                : new TSqlNVarCharValue(value, TSqlNVarCharSize.Max);
        }

        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue BigInt(long? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Int(int? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Bit(bool? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue UniqueIdentifier(Guid? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue DateTimeOffset(DateTimeOffset? value)
        {
            return !value.HasValue
                ? Null()
                : new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a BINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Binary(byte[] value, TSqlBinarySize size)
        {
            return value == null
                ? Null()
                : new TSqlBinaryValue(value, size);
        }

        /// <summary>
        ///     Returns a VARBINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue VarBinary(byte[] value, TSqlVarBinarySize size)
        {
            return value == null
                ? Null()
                : new TSqlVarBinaryValue(value, size);
        }

        /// <summary>
        ///     Returns a VARBINARY parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue VarBinaryMax(byte[] value)
        {
            return value == null
                ? Null()
                : new TSqlVarBinaryValue(value, TSqlVarBinarySize.Max);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlQueryStatement Query(string text, object parameters = null)
        {
            return new TSqlQueryStatement(text, Collect(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQuery(string text, object parameters = null)
        {
            return new TSqlNonQueryStatement(text, Collect(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL projection builder.
        /// </summary>
        /// <returns>A <see cref="TSqlProjectionBuilder" />.</returns>
        public static TSqlProjectionBuilder Projection()
        {
            return new TSqlProjectionBuilder();
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="TSqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="TSqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static TSqlNonQueryStatementComposer Compose(IEnumerable<TSqlNonQueryStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlNonQueryStatementComposer(statements.ToArray());
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="TSqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="TSqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static TSqlNonQueryStatementComposer Compose(params TSqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlNonQueryStatementComposer(statements);
        }

        private static SqlParameter[] Collect(object parameters)
        {
            if (parameters == null)
                return new SqlParameter[0];
            return parameters.
                GetType().
                GetProperties(BindingFlags.Instance | BindingFlags.Public).
                Where(property => typeof (ITSqlParameterValue).IsAssignableFrom(property.PropertyType)).
                Select(property =>
                    ((ITSqlParameterValue) property.GetGetMethod().Invoke(parameters, null)).
                        ToSqlParameter(FormatSqlParameterName(property.Name))).
                ToArray();
        }

        private static string FormatSqlParameterName(string name)
        {
            return "@" + name;
        }
    }
}