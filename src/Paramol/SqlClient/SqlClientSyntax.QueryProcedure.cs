using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Paramol.SqlClient
{
    public partial class SqlClientSyntax
    {
        /// <summary>
        ///     Returns a T-SQL non query stored procedure.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public SqlQueryCommand QueryProcedure(string text, object parameters = null)
        {
            return new SqlQueryCommand(text, CollectFromAnonymousType(parameters), CommandType.StoredProcedure);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public IEnumerable<SqlQueryCommand> QueryProcedureIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return QueryProcedure(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public IEnumerable<SqlQueryCommand> QueryProcedureUnless(bool condition, string text, object parameters = null)
        {
            if (!condition)
                yield return QueryProcedure(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL query stored procedure.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public SqlQueryCommand QueryProcedureFormat(string format, params IDbParameterValue[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return new SqlQueryCommand(format, new DbParameter[0], CommandType.StoredProcedure);
            }
            ThrowIfMaxParameterCountExceeded(parameters);
            return new SqlQueryCommand(
                string.Format(format,
                    parameters.Select((_, index) => (object)FormatDbParameterName("P" + index)).ToArray()),
                parameters.Select((value, index) => value.ToDbParameter(FormatDbParameterName("P" + index))).ToArray(),
                CommandType.StoredProcedure);
        }

        /// <summary>
        ///     Returns a T-SQL query stored procedure if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public IEnumerable<SqlQueryCommand> QueryProcedureFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return QueryProcedureFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL query stored procedure unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlQueryCommand" />.</returns>
        public IEnumerable<SqlQueryCommand> QueryProcedureFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return QueryProcedureFormat(format, parameters);
        }
    }
}
