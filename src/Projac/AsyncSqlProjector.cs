using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paramol;

namespace Projac
{
    public class AsyncSqlProjector
    {
        private readonly SqlProjectionHandler[] _handlers;
        private readonly IAsyncSqlNonQueryStatementExecutor _executor;

        public AsyncSqlProjector(SqlProjectionHandler[] handlers, IAsyncSqlNonQueryStatementExecutor executor)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            if (executor == null) throw new ArgumentNullException("executor");
            _handlers = handlers;
            _executor = executor;
        }

        public Task ProjectAsync(object @event)
        {
            return ProjectAsync(@event, CancellationToken.None);
        }

        public Task ProjectAsync(object @event, CancellationToken cancellationToken)
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