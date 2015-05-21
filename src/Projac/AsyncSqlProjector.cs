using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paramol;
using Paramol.Executors;

namespace Projac
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncSqlProjector
    {
        private readonly SqlProjectionHandlerResolver _resolver;
        private readonly IAsyncSqlNonQueryCommandExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSqlProjector"/> class.
        /// </summary>
        /// <param name="resolver">The handler resolver.</param>
        /// <param name="executor">The command executor.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="resolver"/> or <paramref name="executor"/> is <c>null</c>.</exception>
        public AsyncSqlProjector(SqlProjectionHandlerResolver resolver, IAsyncSqlNonQueryCommandExecutor executor)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (executor == null) throw new ArgumentNullException("executor");

            _resolver = resolver;
            _executor = executor;
        }

        /// <summary>
        /// Projects the specified message asynchronously.
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
        /// Projects the specified message asynchronously.
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

            return _executor.
                ExecuteNonQueryAsync(
                    from handler in _resolver(message)
                    from statement in handler.Handler(message)
                    select statement,
                    cancellationToken);
        }

        /// <summary>
        /// Projects the specified messages to project.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> are <c>null</c>.</exception>
        public Task<int> ProjectAsync(IEnumerable<object> messages)
        {
            return ProjectAsync(messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> are <c>null</c>.</exception>
        public Task<int> ProjectAsync(IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (messages == null) throw new ArgumentNullException("messages");

            return _executor.
                ExecuteNonQueryAsync(
                    from message in messages
                    from handler in _resolver(message)
                    from statement in handler.Handler(message)
                    select statement,
                    cancellationToken);
        }
    }
}