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
                string.Format(format,
                    parameters.Select((_, index) => (object) FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL query statement if the condition is satisified.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryStatement" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
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
        public static IEnumerable<SqlQueryStatement> QueryFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return QueryFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlNonQueryCommand NonQueryFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlNonQueryStatement(format, new DbParameter[0]);
            }
            return new SqlNonQueryStatement(
                string.Format(format,
                    parameters.Select((_, index) => (object) FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
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
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return NonQueryFormat(format, parameters);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer Compose(IEnumerable<SqlNonQueryCommand> commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(commands.ToArray());
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer ComposeIf(bool condition,
            IEnumerable<SqlNonQueryCommand> commands)
        {
            return Compose(condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> unless the condition is
        ///     satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer ComposeUnless(bool condition,
            IEnumerable<SqlNonQueryCommand> commands)
        {
            return Compose(!condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer Compose(params SqlNonQueryCommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(commands);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer ComposeIf(bool condition, params SqlNonQueryCommand[] commands)
        {
            return Compose(condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> unless the condition is
        ///     satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public static SqlNonQueryCommandComposer ComposeUnless(bool condition,
            params SqlNonQueryCommand[] commands)
        {
            return Compose(!condition ? commands : new SqlNonQueryCommand[0]);
        }

        private static string FormatDbParameterName(string name)
        {
            return "@" + name;
        }
    }
}