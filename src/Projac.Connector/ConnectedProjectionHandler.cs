using System;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    ///     Represents a handler of a particular type of message.
    /// </summary>
    public class ConnectedProjectionHandler<TConnection>
    {
        private readonly Type _message;
        private readonly Func<TConnection, object, CancellationToken, Task> _handler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedProjectionHandler{TConnection}" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="message" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public ConnectedProjectionHandler(Type message, Func<TConnection, object, CancellationToken, Task> handler)
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
        public Func<TConnection, object, CancellationToken, Task> Handler
        {
            get { return _handler; }
        }
    }
}
