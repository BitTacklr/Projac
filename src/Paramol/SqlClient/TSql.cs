using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a VARCHAR parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue VarChar(string value, TSqlVarCharSize size)
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
        public static IDbParameterValue Char(string value, TSqlCharSize size)
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
        public static IDbParameterValue VarCharMax(string value)
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
        public static IDbParameterValue NVarChar(string value, TSqlNVarCharSize size)
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
        public static IDbParameterValue NChar(string value, TSqlNCharSize size)
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
        public static IDbParameterValue NVarCharMax(string value)
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
        public static IDbParameterValue Binary(byte[] value, TSqlBinarySize size)
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
        public static IDbParameterValue VarBinary(byte[] value, TSqlVarBinarySize size)
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
        public static IDbParameterValue VarBinaryMax(byte[] value)
        {
            if (value == null)
                return new TSqlVarBinaryNullValue(TSqlVarBinarySize.Max);
            return new TSqlVarBinaryValue(value, TSqlVarBinarySize.Max);
        }

        /// <summary>
        ///     Returns a T-SQL query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryStatement" />.</returns>
        public static SqlQueryStatement QueryFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlQueryStatement(format, new DbParameter[0]);
            }
            return new SqlQueryStatement(
                string.Format(format, parameters.Select((_, index) => (object)FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL query statement if the condition is satisified.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryStatement" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryFormatIf(bool condition, string format, params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return QueryFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryStatement" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryFormatUnless(bool condition, string format, params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return QueryFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlNonQueryStatement NonQueryFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlNonQueryStatement(format, new DbParameter[0]);
            }
            return new SqlNonQueryStatement(
                string.Format(format, parameters.Select((_, index) => (object)FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static IEnumerable<SqlNonQueryStatement> NonQueryFormatIf(bool condition, string format, params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return NonQueryFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static IEnumerable<SqlNonQueryStatement> NonQueryFormatUnless(bool condition, string format, params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return NonQueryFormat(format, parameters);
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer Compose(IEnumerable<SqlNonQueryStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new SqlNonQueryStatementComposer(statements.ToArray());
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer ComposeIf(bool condition, IEnumerable<SqlNonQueryStatement> statements)
        {
            return Compose(condition ? statements : new SqlNonQueryStatement[0]);
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer ComposeUnless(bool condition, IEnumerable<SqlNonQueryStatement> statements)
        {
            return Compose(!condition ? statements : new SqlNonQueryStatement[0]);
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer Compose(params SqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new SqlNonQueryStatementComposer(statements);
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer ComposeIf(bool condition, params SqlNonQueryStatement[] statements)
        {
            return Compose(condition ? statements : new SqlNonQueryStatement[0]);
        }

        /// <summary>
        ///     Starts a composition of statements with the specified <paramref name="statements" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public static SqlNonQueryStatementComposer ComposeUnless(bool condition, params SqlNonQueryStatement[] statements)
        {
            return Compose(!condition ? statements : new SqlNonQueryStatement[0]);
        }

        private static string FormatDbParameterName(string name)
        {
            return "@" + name;
        }
    }
}