using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    public partial class SqlClientSyntax
    {
        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public SqlNonQueryCommand NonQueryStatement(string text, object parameters = null)
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
        public IEnumerable<SqlNonQueryCommand> NonQueryStatementIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return NonQueryStatement(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public IEnumerable<SqlNonQueryCommand> NonQueryStatementUnless(bool condition, string text,
            object parameters = null)
        {
            if (!condition)
                yield return NonQueryStatement(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public SqlNonQueryCommand NonQueryStatementFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlNonQueryCommand(format, new DbParameter[0], CommandType.Text);
            }
            ThrowIfMaxParameterCountExceeded(parameters);
            return new SqlNonQueryCommand(
                string.Format(format,
                    parameters.Select((_, index) => (object)FormatDbParameterName("P" + index)).ToArray()),
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
        public IEnumerable<SqlNonQueryCommand> NonQueryStatementFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return NonQueryStatementFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public IEnumerable<SqlNonQueryCommand> NonQueryStatementFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return NonQueryStatementFormat(format, parameters);
        }
    }
}
