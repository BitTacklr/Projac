using System;
using System.Collections.Generic;
using System.Linq;
using Paramol;
using Paramol.Executors;

namespace Projac
{
    /// <summary>
    /// Projects a single message in a synchronous manner to the matching handlers.
    /// </summary>
    public class SqlProjector
    {
        private readonly SqlProjectionHandler[] _handlers;
        private readonly ISqlNonQueryCommandExecutor _executor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <param name="executor">The command executor.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> or <paramref name="executor"/> is <c>null</c>.</exception>
        public SqlProjector(SqlProjectionHandler[] handlers, ISqlNonQueryCommandExecutor executor)
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
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="message"/> is <c>null</c>.</exception>
        public int Project(object message)
        {
            if (message == null) throw new ArgumentNullException("message");

            return _executor.ExecuteNonQuery(
                _handlers.
                    Where(handler => handler.Message == message.GetType()).
                    SelectMany(handler => handler.Handler(message)));
        }

        /// <summary>
        /// Projects the specified messages.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> are <c>null</c>.</exception>
        public int Project(IEnumerable<object> messages)
        {
            if (messages == null) 
                throw new ArgumentNullException("messages");

            return _executor.
                ExecuteNonQuery(
                    from message in messages
                    from handler in _handlers
                    where handler.Message == message.GetType()
                    from statement in handler.Handler(message)
                    select statement);
        }
    }
}
