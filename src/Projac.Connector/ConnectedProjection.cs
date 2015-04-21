using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    ///     Represent a connected projection.
    /// </summary>
    public abstract class ConnectedProjection<TConnection>
    {
        private readonly List<ConnectedProjectionHandler<TConnection>> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedProjection{TConnection}" /> class.
        /// </summary>
        protected ConnectedProjection()
        {
            _handlers = new List<ConnectedProjectionHandler<TConnection>>();
        }

        private ConnectedProjection(ConnectedProjectionHandler<TConnection>[] handlers)
        {
            _handlers = new List<ConnectedProjectionHandler<TConnection>>(handlers);
        }
 
        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ConnectedProjectionHandler<TConnection>(
                    typeof(TMessage),
                    (connection, message, token) => handler(connection, (TMessage)message)));
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ConnectedProjectionHandler<TConnection>(
                    typeof(TMessage),
                    (connection, message, token) => handler(connection, (TMessage)message, token)));
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public ConnectedProjectionHandler<TConnection>[] Handlers
        {
            get { return _handlers.ToArray(); }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="ConnectedProjection{TConnection}"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public ConnectedProjection<TConnection> Concat(ConnectedProjection<TConnection> projection)
        {
            if (projection == null)
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="ConnectedProjection{TConnection}"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public ConnectedProjection<TConnection> Concat(ConnectedProjectionHandler<TConnection> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new ConnectedProjectionHandler<TConnection>[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new ConnectedProjection<TConnection>(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="ConnectedProjection{TConnection}"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public ConnectedProjection<TConnection> Concat(ConnectedProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new ConnectedProjectionHandler<TConnection>[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new ConnectedProjection<TConnection>(concatenated);
        }
    }
}