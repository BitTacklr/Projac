using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    ///     Represents a fluent syntax to build up a set of <see cref="ConnectedProjectionHandler{TConnection}" />.
    /// </summary>
    public class ConnectedProjectionBuilder<TConnection>
    {
        private readonly ConnectedProjectionHandler<TConnection>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedProjectionBuilder{TConnection}" /> class.
        /// </summary>
        public ConnectedProjectionBuilder() :
            this(new ConnectedProjectionHandler<TConnection>[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedProjectionBuilder{TConnection}" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlers"/> is <c>null</c>.</exception>
        public ConnectedProjectionBuilder(ConnectedProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <returns>A <see cref="ConnectedProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public ConnectedProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new ConnectedProjectionBuilder<TConnection>(
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
        ///     Specifies the non query command array returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <returns>A <see cref="ConnectedProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public ConnectedProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new ConnectedProjectionBuilder<TConnection>(
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
        ///     Builds a set of the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="ConnectedProjectionHandler{TConnection}" /> array.</returns>
        public ConnectedProjectionHandler<TConnection>[] Build()
        {
            return _handlers;
        }
    }
}