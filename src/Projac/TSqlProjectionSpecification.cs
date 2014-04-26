using System;
using System.Collections.Generic;

namespace Projac
{
    /// <summary>
    ///     Represent a specification of a projection.
    /// </summary>
    public class TSqlProjectionSpecification
    {
        private readonly TSqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlProjectionSpecification" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public TSqlProjectionSpecification(TSqlProjectionHandler[] handlers)
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
        public IReadOnlyCollection<TSqlProjectionHandler> Handlers
        {
            get { return _handlers; }
        }
    }
}