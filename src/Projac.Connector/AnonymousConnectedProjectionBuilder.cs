using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    ///     Represents a fluent syntax to build up a set of <see cref="ConnectedProjectionHandler{TConnection}" />.
    /// </summary>
    public class AnonymousConnectedProjectionBuilder<TConnection>
    {
        private readonly ConnectedProjectionHandler<TConnection>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousConnectedProjectionBuilder{TConnection}" /> class.
        /// </summary>
        public AnonymousConnectedProjectionBuilder() :
            this(new ConnectedProjectionHandler<TConnection>[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousConnectedProjectionBuilder{TConnection}" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlers"/> is <c>null</c>.</exception>
        public AnonymousConnectedProjectionBuilder(ConnectedProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously.</param>
        /// <returns>A <see cref="AnonymousConnectedProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousConnectedProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousConnectedProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ConnectedProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message synchronously.</param>
        /// <returns>A <see cref="AnonymousConnectedProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousConnectedProjectionBuilder<TConnection> When<TMessage>(Action<TConnection, TMessage> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            return new AnonymousConnectedProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ConnectedProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) =>
                            {
                                handler(connection, (TMessage) message);
                                return Task.FromResult<object>(null);
                            })
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously and with cancellation support.</param>
        /// <returns>A <see cref="AnonymousConnectedProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousConnectedProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousConnectedProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ConnectedProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds an <see cref="AnonymousConnectedProjection{TConnection}"/> using the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="AnonymousConnectedProjection{TConnection}" /> array.</returns>
        public AnonymousConnectedProjection<TConnection> Build()
        {
            return new AnonymousConnectedProjection<TConnection>(_handlers);
        }
    }
}