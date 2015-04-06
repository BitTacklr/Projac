using System;

namespace Projac.RavenDB
{
    /// <summary>
    ///     Represent a Raven backed projection.
    /// </summary>
    public class RavenProjection
    {
        /// <summary>
        /// Returns a <see cref="RavenProjection"/> instance without handlers.
        /// </summary>
        public static readonly RavenProjection Empty = new RavenProjection(new RavenProjectionHandler[0]);

        private readonly RavenProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RavenProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public RavenProjection(RavenProjectionHandler[] handlers)
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
        public RavenProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="RavenProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public RavenProjection Concat(RavenProjection projection)
        {
            if (projection == null)
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="RavenProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public RavenProjection Concat(RavenProjectionHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new RavenProjectionHandler[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new RavenProjection(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="RavenProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public RavenProjection Concat(RavenProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new RavenProjectionHandler[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new RavenProjection(concatenated);
        }

        /// <summary>
        /// Creates a <see cref="RavenProjectionBuilder"/> based on the handlers of this projection.
        /// </summary>
        /// <returns>A <see cref="RavenProjectionBuilder"/>.</returns>
        public RavenProjectionBuilder ToBuilder()
        {
            return new RavenProjectionBuilder(this);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="RavenProjection"/> to <see><cref>RavenProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RavenProjectionHandler[](RavenProjection instance)
        {
            return instance.Handlers;
        }
    }
}