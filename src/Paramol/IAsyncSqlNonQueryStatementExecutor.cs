using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol
{
    /// <summary>
    /// Represents the asynchronous execution of a set of <see cref="SqlNonQueryStatement">statements</see> against a data source.
    /// </summary>
    public interface IAsyncSqlNonQueryStatementExecutor
    {
        /// <summary>
        /// Executes the specified statements asynchronously.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>A <see cref="Task"/> that will return the number of <see cref="SqlNonQueryStatement">statements</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when <paramref name="statements"/> are <c>null</c>.</exception>
        Task<int> ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements);

        /// <summary>
        /// Executes the specified statements asynchronously.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> that will return the number of <see cref="SqlNonQueryStatement">statements</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when <paramref name="statements" /> are <c>null</c>.</exception>
        Task<int> ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements, CancellationToken cancellationToken);
    }
}