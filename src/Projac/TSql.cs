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
    public static partial class TSql
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
        ///     Returns a T-SQL query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="TSqlQueryStatement" />.</returns>
        public static TSqlQueryStatement QueryFormat(string format, params ITSqlParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new TSqlQueryStatement(format, new SqlParameter[0]);
            }
            return new TSqlQueryStatement(
                string.Format(format, parameters.Select((_, index) => (object)FormatSqlParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToSqlParameter(FormatSqlParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQueryFormat(string format, params ITSqlParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new TSqlNonQueryStatement(format, new SqlParameter[0]);
            }
            return new TSqlNonQueryStatement(
                string.Format(format, parameters.Select((_, index) => (object) FormatSqlParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToSqlParameter(FormatSqlParameterName("P" + index))).ToArray());
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

        private static string FormatSqlParameterName(string name)
        {
            return "@" + name;
        }
    }
}