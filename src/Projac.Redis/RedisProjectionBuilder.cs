using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Projac.Redis
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="RedisProjection" />.
    /// </summary>
    public class RedisProjectionBuilder
    {
        private readonly RedisProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedisProjectionBuilder" /> class.
        /// </summary>
        public RedisProjectionBuilder() :
            this(new RedisProjectionHandler[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedisProjectionBuilder" /> class.
        /// </summary>
        public RedisProjectionBuilder(RedisProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _handlers = projection.Handlers;
        }

        RedisProjectionBuilder(RedisProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="RedisProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public RedisProjectionBuilder When<TMessage>(Func<ConnectionMultiplexer, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new RedisProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new RedisProjectionHandler(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="RedisProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public RedisProjectionBuilder When<TMessage>(Func<ConnectionMultiplexer, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new RedisProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new RedisProjectionHandler(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds a projection specification based on the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="RedisProjection" />.</returns>
        public RedisProjection Build()
        {
            return new RedisProjection(_handlers);
        }
    }
}