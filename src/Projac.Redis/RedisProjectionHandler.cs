using System;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Projac.Redis
{
    /// <summary>
    ///     Represents a handler of a particular type of message.
    /// </summary>
    public class RedisProjectionHandler
    {
        private readonly Type _message;
        private readonly Func<ConnectionMultiplexer, object, CancellationToken, Task> _handler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedisProjectionHandler" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="message" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public RedisProjectionHandler(Type message, Func<ConnectionMultiplexer, object, CancellationToken, Task> handler)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (handler == null) throw new ArgumentNullException("handler");
            _message = message;
            _handler = handler;
        }

        /// <summary>
        ///     The type of message to handle.
        /// </summary>
        public Type Message
        {
            get { return _message; }
        }

        /// <summary>
        ///     The function that handles the message.
        /// </summary>
        public Func<ConnectionMultiplexer, object, CancellationToken, Task> Handler
        {
            get { return _handler; }
        }
    }
}
