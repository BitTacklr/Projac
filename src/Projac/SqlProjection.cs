using System;
using System.Collections.Generic;

namespace Projac
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public class SqlProjection
    {
        private readonly SqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public SqlProjection(SqlProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public IReadOnlyCollection<SqlProjectionHandler> Handlers
        {
            get { return _handlers; }
        }
    }
}