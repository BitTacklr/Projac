using System;
using System.Collections;
using System.Collections.Generic;

namespace Projac.Connector
{
    /// <summary>
    ///     Represent an anonymous connected projection.
    /// </summary>
    public class AnonymousConnectedProjection<TConnection> : IEnumerable<ConnectedProjectionHandler<TConnection>>
    {
        private readonly ConnectedProjectionHandler<TConnection>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousConnectedProjection{TConnection}" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="handlers" /> are <c>null</c>.
        /// </exception>
        public AnonymousConnectedProjection(ConnectedProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null) 
                throw new ArgumentNullException("handlers");

            _handlers = handlers;
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public ConnectedProjectionHandler<TConnection>[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AnonymousConnectedProjection{TConnection}"/> to <see><cref>ConnectedProjectionHandler{TConnection}</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ConnectedProjectionHandler<TConnection>[](AnonymousConnectedProjection<TConnection> instance)
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