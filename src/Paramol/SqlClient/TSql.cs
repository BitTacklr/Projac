using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlQueryStatement Query(string text, object parameters = null)
        {
            return new SqlQueryStatement(text, CollectFromAnonymousType(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return Query(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryUnless(bool condition, string text, object parameters = null)
        {
            if (!condition)
                yield return Query(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlNonQueryCommand NonQuery(string text, object parameters = null)
        {
            return new SqlNonQueryCommand(text, CollectFromAnonymousType(parameters), CommandType.Text);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return NonQuery(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryUnless(bool condition, string text,
            object parameters = null)
        {
            if (!condition)
                yield return NonQuery(text, parameters);
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
            ThrowIfMaxParameterCountExceeded(parameters);
            return new SqlQueryStatement(
                string.Format(format,
                    parameters.Select((_, index) => (object) FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray());
        }

        /// <summary>
        ///     Returns a T-SQL query statement if the condition is satisfied.
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
                return new SqlNonQueryCommand(format, new DbParameter[0], CommandType.Text);
            }
            ThrowIfMaxParameterCountExceeded(parameters);
            return new SqlNonQueryCommand(
                string.Format(format,
                    parameters.Select((_, index) => (object) FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray(),
                CommandType.Text);
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

        private static DbParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return ThrowIfMaxParameterCountExceeded(
                parameters.
                    GetType().
                    GetProperties(BindingFlags.Instance | BindingFlags.Public).
                    Where(property => typeof(IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                    Select(property =>
                        ((IDbParameterValue)property.GetGetMethod().Invoke(parameters, null)).
                            ToDbParameter(FormatDbParameterName(property.Name))).
                    ToArray());
        }

        private static DbParameter[] ThrowIfMaxParameterCountExceeded(DbParameter[] parameters)
        {
            if (parameters.Length > Limits.MaxParameterCount)
                throw new ArgumentException(
                    string.Format("The parameter count is limited to {0}.", Limits.MaxParameterCount),
                    "parameters");
            return parameters;
        }

        private static void ThrowIfMaxParameterCountExceeded(IDbParameterValue[] parameters)
        {
            if (parameters.Length > Limits.MaxParameterCount)
                throw new ArgumentException(
                    string.Format("The parameter count is limited to {0}.", Limits.MaxParameterCount),
                    "parameters");
        }

        private static string FormatDbParameterName(string name)
        {
            return "@" + name;
        }
    }
}