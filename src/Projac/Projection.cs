using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public abstract class Projection<TConnection> : IEnumerable<ProjectionHandler<TConnection>>
    {
        private readonly List<ProjectionHandler<TConnection>> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Projection{TConnection}" /> class.
        /// </summary>
        protected Projection()
        {
            _handlers = new List<ProjectionHandler<TConnection>>();
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
                new ProjectionHandler<TConnection>(
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
                new ProjectionHandler<TConnection>(
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
                new ProjectionHandler<TConnection>(
                    typeof(TMessage),
                    (connection, message, token) => handler(connection, (TMessage)message, token)));
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public ProjectionHandler<TConnection>[] Handlers
        {
            get { return _handlers.ToArray(); }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Projection{TConnection}"/> to <see><cref>ProjectionHandler{TConnection}</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ProjectionHandler<TConnection>[](Projection<TConnection> instance)
        {
            return instance.Handlers;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a copy of the handlers.
        /// </summary>
        /// <returns>
        /// An <see cref="ProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public ProjectionHandlerEnumerator<TConnection> GetEnumerator()
        {
            return new ProjectionHandlerEnumerator<TConnection>(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="ProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<ProjectionHandler<TConnection>> IEnumerable<ProjectionHandler<TConnection>>.GetEnumerator()
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