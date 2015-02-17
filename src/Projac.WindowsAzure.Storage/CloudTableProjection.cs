using System;

namespace Projac.WindowsAzure.Storage
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public class CloudTableProjection
    {
        /// <summary>
        /// Returns a <see cref="CloudTableProjection"/> instance without handlers.
        /// </summary>
        public static readonly CloudTableProjection Empty = new CloudTableProjection(new CloudTableProjectionHandler[0]);

        private readonly CloudTableProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloudTableProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public CloudTableProjection(CloudTableProjectionHandler[] handlers)
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
        public CloudTableProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="CloudTableProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public CloudTableProjection Concat(CloudTableProjection projection)
        {
            if (projection == null)
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="CloudTableProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public CloudTableProjection Concat(CloudTableProjectionHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new CloudTableProjectionHandler[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new CloudTableProjection(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="CloudTableProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public CloudTableProjection Concat(CloudTableProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new CloudTableProjectionHandler[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new CloudTableProjection(concatenated);
        }

        /// <summary>
        /// Creates a <see cref="CloudTableProjectionBuilder"/> based on the handlers of this projection.
        /// </summary>
        /// <returns>A <see cref="CloudTableProjectionBuilder"/>.</returns>
        public CloudTableProjectionBuilder ToBuilder()
        {
            return new CloudTableProjectionBuilder(this);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CloudTableProjection"/> to <see><cref>CloudTableProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CloudTableProjectionHandler[](CloudTableProjection instance)
        {
            return instance.Handlers;
        }
    }
}