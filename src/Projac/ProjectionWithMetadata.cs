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
    public abstract partial class Projection<TConnection, TMetadata> : IEnumerable<ProjectionHandler<TConnection, TMetadata>>
    {
        private readonly List<ProjectionHandler<TConnection, TMetadata>> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Projection{TConnection}" /> class.
        /// </summary>
        protected Projection()
        {
            _handlers = new List<ProjectionHandler<TConnection, TMetadata>>();
        }
 
        /// <summary>
        ///     Specifies the asynchronous message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void Handle<TMessage>(Func<TConnection, TMessage, TMetadata, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ProjectionHandler<TConnection, TMetadata>(
                    typeof(TMessage),
                    (connection, message, metadata, token) => handler(connection, (TMessage)message, metadata)));
        }

        /// <summary>
        ///     Specifies the synchronous message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void Handle<TMessage>(Action<TConnection, TMessage, TMetadata> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ProjectionHandler<TConnection, TMetadata>(
                    typeof(TMessage),
                    (connection, message, metadata, token) =>
                    {
                        handler(connection, (TMessage) message, metadata);
                        return Task.CompletedTask;
                    }));
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void Handle<TMessage>(Func<TConnection, TMessage, TMetadata, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(
                new ProjectionHandler<TConnection, TMetadata>(
                    typeof(TMessage),
                    (connection, message, metadata, token) => handler(connection, (TMessage)message, metadata, token)));
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public ProjectionHandler<TConnection, TMetadata>[] Handlers
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
        public static implicit operator ProjectionHandler<TConnection, TMetadata>[](Projection<TConnection, TMetadata> instance)
        {
            return instance.Handlers;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a copy of the handlers.
        /// </summary>
        /// <returns>
        /// An <see cref="ProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public ProjectionHandlerEnumerator<TConnection, TMetadata> GetEnumerator()
        {
            return new ProjectionHandlerEnumerator<TConnection, TMetadata>(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="ProjectionHandlerEnumerator{TConnection}" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<ProjectionHandler<TConnection, TMetadata>> IEnumerable<ProjectionHandler<TConnection, TMetadata>>.GetEnumerator()
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