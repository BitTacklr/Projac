using System;
using System.Linq;
using Paramol;

namespace Projac
{
    /// <summary>
    /// Projects a single event in a synchronous manner to the matching handlers.
    /// </summary>
    public class SqlProjector
    {
        private readonly SqlProjectionHandler[] _handlers;
        private readonly ISqlNonQueryStatementExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <param name="executor">The statement executor.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> or <paramref name="executor"/> is <c>null</c>.</exception>
        public SqlProjector(SqlProjectionHandler[] handlers, ISqlNonQueryStatementExecutor executor)
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
        public int Project(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            return _executor.Execute(
                _handlers.
                    Where(handler => handler.Event == @event.GetType()).
                    SelectMany(handler => handler.Handler(@event)));
        }
    }
}
