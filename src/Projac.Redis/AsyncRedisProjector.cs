using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Projac.Redis
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncRedisProjector
    {
        private readonly Dictionary<Type, RedisProjectionHandler[]> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRedisProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> is <c>null</c>.</exception>
        public AsyncRedisProjector(RedisProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The Redis connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(ConnectionMultiplexer connection, object message)
        {
            return ProjectAsync(connection, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The Redis connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public async Task ProjectAsync(ConnectionMultiplexer connection, object message, CancellationToken cancellationToken)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (message == null) throw new ArgumentNullException("message");

            RedisProjectionHandler[] handlers;
            if (_handlers.TryGetValue(message.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    await handler.Handler(connection, message, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="connection">The Redis connection used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(ConnectionMultiplexer connection, IEnumerable<object> messages)
        {
            return ProjectAsync(connection, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="connection">The Redis connection used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public async Task ProjectAsync(ConnectionMultiplexer connection, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (messages == null) throw new ArgumentNullException("messages");

            foreach (var message in messages)
            {
                RedisProjectionHandler[] handlers;
                if (!_handlers.TryGetValue(message.GetType(), out handlers)) 
                    continue;

                foreach (var handler in handlers)
                {
                    await handler.Handler(connection, message, cancellationToken);
                }
            }
        }
    }
}
