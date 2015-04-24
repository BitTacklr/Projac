using System;
using System.Collections.Generic;
using System.Linq;
using Paramol;
using Paramol.Executors;

namespace Projac
{
    /// <summary>
    /// Projects a single message or a set of messages in a synchronous manner to the matching handlers.
    /// </summary>
    public class SqlProjector
    {
        private readonly SqlProjectionHandlerIndexedByMessageTypeCache _cache;
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

            _cache = new SqlProjectionHandlerIndexedByMessageTypeCache(handlers);
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

            return _executor.
                ExecuteNonQuery(
                    from handler in _cache.GetOrAdd(message.GetType())
                    from statement in handler.Handler(message)
                    select statement);
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
                    from handler in _cache.GetOrAdd(message.GetType())
                    from statement in handler.Handler(message)
                    select statement);
        }

        class SqlProjectionHandlerIndexedByMessageTypeCache
        {
            private readonly SqlProjectionHandler[] _handlers;
            private readonly Dictionary<Type, SqlProjectionHandler[]> _cache;

            public SqlProjectionHandlerIndexedByMessageTypeCache(SqlProjectionHandler[] handlers)
            {
                _handlers = handlers;
                _cache = new Dictionary<Type, SqlProjectionHandler[]>();
            }

            public SqlProjectionHandler[] GetOrAdd(Type message)
            {
                SqlProjectionHandler[] handlers;
                if (!_cache.TryGetValue(message, out handlers))
                {
                    handlers = Array.FindAll(_handlers, handler => handler.Message.IsAssignableFrom(message));
                    _cache.Add(message, handlers);
                }
                return handlers;
            }
        }
    }
}
