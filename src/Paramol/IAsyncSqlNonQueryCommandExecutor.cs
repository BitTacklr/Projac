using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol
{
    /// <summary>
    ///     Represents the asynchronous execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data
    ///     source.
    /// </summary>
    public interface IAsyncSqlNonQueryCommandExecutor
    {
        /// <summary>
        ///     Executes the specified commands asynchronously.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        Task<int> ExecuteAsync(IEnumerable<SqlNonQueryCommand> commands);

        /// <summary>
        ///     Executes the specified commands asynchronously.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        Task<int> ExecuteAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken);
    }
}