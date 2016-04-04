using System.Collections.Generic;
using System.Data;
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
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlQueryCommand QueryStatement(string text, object parameters = null)
        {
            return new SqlQueryCommand(text, CollectFromAnonymousType(parameters), CommandType.Text);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryCommand> QueryStatementIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return QueryStatement(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryCommand> QueryStatementUnless(bool condition, string text, object parameters = null)
        {
            if (!condition)
                yield return QueryStatement(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public static SqlQueryCommand QueryStatementFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlQueryCommand(format, new DbParameter[0], CommandType.Text);
            }
            ThrowIfMaxParameterCountExceeded(parameters);
            return new SqlQueryCommand(
                string.Format(format,
                    parameters.Select((_, index) => (object) FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray(),
                CommandType.Text);
        }

        /// <summary>
        ///     Returns a T-SQL query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryCommand> QueryStatementFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return QueryStatementFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryCommand> QueryStatementFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return QueryStatementFormat(format, parameters);
        }
    }
}