using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represent a SQL non query stored procedure.
    /// </summary>
    public class SqlNonQueryProcedure : SqlNonQueryCommand
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryProcedure" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown when <paramref name="text" /> or <paramref name="parameters" />
        ///     is <c>null</c>.
        /// </exception>
        public SqlNonQueryProcedure(string text, DbParameter[] parameters)
            : base(text, parameters, CommandType.StoredProcedure)
        {
        }
    }
}