using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Projac.Sql;

namespace Projac.SqlClient.Legacy
{
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a T-SQL non query stored procedure.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlNonQueryCommand NonQueryProcedure(string text, object parameters = null)
        {
            return new SqlNonQueryCommand(text, CollectFromAnonymousType(parameters), CommandType.StoredProcedure);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryProcedureIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return NonQueryProcedure(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryProcedureUnless(bool condition, string text,
            object parameters = null)
        {
            if (!condition)
                yield return NonQueryProcedure(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure.
        /// </summary>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlNonQueryCommand NonQueryProcedureFormat(string format, params IDbParameterValue[] parameters)
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
                CommandType.StoredProcedure);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryProcedureFormatIf(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (condition)
                yield return NonQueryProcedureFormat(format, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query stored procedure unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="format">The text with positional parameters to be formatted.</param>
        /// <param name="parameters">The positional parameter values.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryProcedureFormatUnless(bool condition, string format,
            params IDbParameterValue[] parameters)
        {
            if (!condition)
                yield return NonQueryProcedureFormat(format, parameters);
        }
    }
}