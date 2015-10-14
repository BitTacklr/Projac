using System;
using System.Collections;
using System.Collections.Generic;

namespace Projac.Connector
{
    /// <summary>
    /// Represents a <see cref="ConnectedProjectionHandler{TConnection}"/> array enumerator.
    /// </summary>
    public class ConnectedProjectionHandlerEnumerator<TConnection> : IEnumerator<ConnectedProjectionHandler<TConnection>>
    {
        private readonly ConnectedProjectionHandler<TConnection>[] _handlers;
        private int _index;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedProjectionHandlerEnumerator{TConnection}"/> class.
        /// </summary>
        /// <param name="handlers">The handlers to enumerate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public ConnectedProjectionHandlerEnumerator(ConnectedProjectionHandler<TConnection>[] handlers)
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