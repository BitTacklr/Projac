using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paramol;

namespace Projac
{
    /// <summary>
    /// Projects a single event in an asynchronous manner to the matching handlers.
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
        /// Projects the specified event.
        /// </summary>
        /// <param name="event">The event to project.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        public Task<int> ProjectAsync(object @event)
        {
            return ProjectAsync(@event, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified event.
        /// </summary>
        /// <param name="event">The event to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="event"/> is <c>null</c>.</exception>
        public Task<int> ProjectAsync(object @event, CancellationToken cancellationToken)
        {
            if (@event == null) throw new ArgumentNullException("event");

            return _executor.ExecuteAsync(
                _handlers.
                    Where(handler => handler.Event == @event.GetType()).
                    SelectMany(handler => handler.Handler(@event)),
                cancellationToken);
        }
    }
}