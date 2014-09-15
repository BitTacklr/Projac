using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol.Executors
{
    /// <summary>
    ///     Represents the synchronous execution of a <see cref="SqlQueryCommand">command</see> against a data
    ///     source.
    /// </summary>
    public interface IAsyncSqlQueryCommandExecutor
    {
        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="DbDataReader">data reader</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        Task<DbDataReader> ExecuteReaderAsync(SqlQueryCommand command);

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="DbDataReader">data reader</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        Task<DbDataReader> ExecuteReaderAsync(SqlQueryCommand command, CancellationToken cancellationToken);

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="object">scalar value</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        Task<object> ExecuteScalarAsync(SqlQueryCommand command);

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="object">scalar value</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        Task<object> ExecuteScalarAsync(SqlQueryCommand command, CancellationToken cancellationToken);
    }
}