using System;
using System.Linq;
using Paramol;

namespace Projac
{
    public class SqlProjector
    {
        private readonly SqlProjectionHandler[] _handlers;
        private readonly ISqlNonQueryStatementExecutor _executor;

        public SqlProjector(SqlProjectionHandler[] handlers, ISqlNonQueryStatementExecutor executor)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            if (executor == null) throw new ArgumentNullException("executor");
            _handlers = handlers;
            _executor = executor;
        }

        public void Project(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");

            _executor.Execute(
                _handlers.
                    Where(handler => handler.Event == @event.GetType()).
                    SelectMany(handler => handler.Handler(@event)));
        }
    }
}
