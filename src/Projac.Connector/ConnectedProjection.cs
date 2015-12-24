using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    ///     Represent a connected projection.
    /// </summary>
    public abstract class ConnectedProjection<TConnection> : IEnumerable<ConnectedProjectionHandler<TConnection>>
    {
        private readonly List<ConnectedProjectionHandler<TConnection>> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedProjection{TConnection}" /> class.
        /// </summary>
        protected ConnectedProjection()
        {
            _handlers = new List<ConnectedProjectionHandler<TConnection>>();
        }
 
        /// <summary>
        ///     Specifies the asynchronous message handler to be invoked when a particular message occurs.
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
        ///     Specifies the synchronous message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Action<TConnection, TMessage> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ConnectedProjectionHandler<TConnection>(
                    typeof(TMessage),
                    (connection, message, token) =>
                    {
                        handler(connection, (TMessage) message);
                        return Task.FromResult<object>(null);
                    }));
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
        /// Performs an implicit conversion from <see cref="ConnectedProjection{TConnection}"/> to <see><cref>ConnectedProjectionHandler{TConnection}</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ConnectedProjectionHandler<TConnection>[](ConnectedProjection<TConnection> instance)
        {
            return instance.Handlers;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a copy of the handlers.
        /// </summary>
        /// <returns>
        /// An <see cref="ConnectedProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public ConnectedProjectionHandlerEnumerator<TConnection> GetEnumerator()
        {
            return new ConnectedProjectionHandlerEnumerator<TConnection>(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="ConnectedProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<ConnectedProjectionHandler<TConnection>> IEnumerable<ConnectedProjectionHandler<TConnection>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}