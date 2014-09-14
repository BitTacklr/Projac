using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paramol;
using Paramol.Executors;

namespace Projac
{
    /// <summary>
    /// Projects a single message in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncSqlProjector
    {
        private readonly SqlProjectionHandler[] _handlers;
        private readonly IAsyncSqlNonQueryCommandExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <param name="executor">The command executor.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> or <paramref name="executor"/> is <c>null</c>.</exception>
        public AsyncSqlProjector(SqlProjectionHandler[] handlers, IAsyncSqlNonQueryCommandExecutor executor)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            if (executor == null) throw new ArgumentNullException("executor");
            _handlers = handlers;
            _executor = executor;
        }

        /// <summary>
        /// Projects the specified message.
        /// </summary>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="message"/> is <c>null</c>.</exception>
        public Task<int> ProjectAsync(object message)
        {
            return ProjectAsync(message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message.
        /// </summary>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="message"/> is <c>null</c>.</exception>
        public Task<int> ProjectAsync(object message, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException("message");

            return _executor.ExecuteNonQueryAsync(
                _handlers.
                    Where(handler => handler.Message == message.GetType()).
                    SelectMany(handler => handler.Handler(message)),
                cancellationToken);
        }
    }
}