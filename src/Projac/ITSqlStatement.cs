using System.Data.SqlClient;

namespace Projac
{
    /// <summary>
    /// Represents a T-SQL statement.
    /// </summary>
    public interface ITSqlStatement
    {
        /// <summary>
        /// Writes the text and parameters to the specified <paramref name="command"/>.
        /// </summary>
        /// <param name="command">The command to write to.</param>
        void WriteTo(SqlCommand command);
    }
}