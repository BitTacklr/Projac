using System.Data.Common;

namespace Paramol.Executors
{
    /// <summary>
    ///     Represents the synchronous execution of a <see cref="SqlQueryCommand">command</see> against a data
    ///     source.
    /// </summary>
    public interface ISqlQueryCommandExecutor
    {
        /// <summary>
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="DbDataReader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        DbDataReader ExecuteReader(SqlQueryCommand command);

        /// <summary>
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="object">scalar value</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        object ExecuteScalar(SqlQueryCommand command);
    }
}