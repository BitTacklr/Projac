using System;
using System.Collections.Generic;

namespace Projac
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public class TSqlProjection
    {
        /// <summary>
        ///     Represents an empty projection instance.
        /// </summary>
        public static readonly TSqlProjection Empty = new TSqlProjection(new TSqlProjectionHandler[0]);

        private readonly TSqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public TSqlProjection(TSqlProjectionHandler[] handlers)
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