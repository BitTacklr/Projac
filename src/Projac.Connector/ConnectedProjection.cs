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
        /// An <see cref="Enumerator" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="Enumerator" /> that can be used to iterate through the collection.
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

        /// <summary>
        /// Represents a <see cref="ConnectedProjectionHandler{TConnection}"/> array enumerator.
        /// </summary>
        public class Enumerator : IEnumerator<ConnectedProjectionHandler<TConnection>>
        {
            private readonly ConnectedProjectionHandler<TConnection>[] _handlers;
            private int _index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> class.
            /// </summary>
            /// <param name="handlers">The handlers to enumerate.</param>
            /// <exception cref="ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
            public Enumerator(ConnectedProjectionHandler<TConnection>[] handlers)
            {
                if (handlers == null) throw new ArgumentNullException("handlers");
                _handlers = handlers;
                _index = -1;
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                return _index < _handlers.Length &&
                    ++_index < _handlers.Length;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <exception cref="System.InvalidOperationException">
            /// Enumeration has not started. Call MoveNext.
            /// or
            /// Enumeration has already ended. Call Reset.
            /// </exception>
            public ConnectedProjectionHandler<TConnection> Current
            {
                get
                {
                    if (_index == -1)
                        throw new InvalidOperationException("Enumeration has not started. Call MoveNext.");
                    if (_index == _handlers.Length)
                        throw new InvalidOperationException("Enumeration has already ended. Call Reset.");

                    return _handlers[_index];
                }
            }

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            object IEnumerator.Current
            {
                get { return Current; }
            }

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }
        }
    }
}