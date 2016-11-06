using System;
using System.Collections;
using System.Collections.Generic;

namespace Projac
{
    /// <summary>
    ///     Represent an anonymous projection.
    /// </summary>
    public class AnonymousProjection<TConnection> : IEnumerable<ProjectionHandler<TConnection>>
    {
        private readonly ProjectionHandler<TConnection>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousProjection{TConnection}" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="handlers" /> are <c>null</c>.
        /// </exception>
        public AnonymousProjection(ProjectionHandler<TConnection>[] handlers)
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
        public ProjectionHandler<TConnection>[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AnonymousProjection{TConnection}"/> to <see><cref>ProjectionHandler{TConnection}</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ProjectionHandler<TConnection>[](AnonymousProjection<TConnection> instance)
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