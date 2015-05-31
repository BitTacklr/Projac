using System;
using System.Collections;
using System.Collections.Generic;

namespace Projac
{
    /// <summary>
    ///     Represent an anonymous SQL projection.
    /// </summary>
    public class AnonymousSqlProjection : IEnumerable<SqlProjectionHandler>
    {
        private readonly SqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousSqlProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers associated with this projection.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public AnonymousSqlProjection(SqlProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this projection.
        /// </value>
        public SqlProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="AnonymousSqlProjection"/> to <see><cref>SqlProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SqlProjectionHandler[](AnonymousSqlProjection instance)
        {
            return instance.Handlers;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a copy of the handlers.
        /// </summary>
        /// <returns>
        /// An <see cref="SqlProjectionHandlerEnumerator" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public SqlProjectionHandlerEnumerator GetEnumerator()
        {
            return new SqlProjectionHandlerEnumerator(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="SqlProjectionHandlerEnumerator" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<SqlProjectionHandler> IEnumerable<SqlProjectionHandler>.GetEnumerator()
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