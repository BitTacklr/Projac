using System;

namespace Projac.Redis
{
    /// <summary>
    ///     Represent a Redis backed projection.
    /// </summary>
    public class RedisProjection
    {
        /// <summary>
        /// Returns a <see cref="RedisProjection"/> instance without handlers.
        /// </summary>
        public static readonly RedisProjection Empty = new RedisProjection(new RedisProjectionHandler[0]);

        private readonly RedisProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RedisProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public RedisProjection(RedisProjectionHandler[] handlers)
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
        public RedisProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="RedisProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public RedisProjection Concat(RedisProjection projection)
        {
            if (projection == null)
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="RedisProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public RedisProjection Concat(RedisProjectionHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new RedisProjectionHandler[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new RedisProjection(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="RedisProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public RedisProjection Concat(RedisProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new RedisProjectionHandler[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new RedisProjection(concatenated);
        }

        /// <summary>
        /// Creates a <see cref="RedisProjectionBuilder"/> based on the handlers of this projection.
        /// </summary>
        /// <returns>A <see cref="RedisProjectionBuilder"/>.</returns>
        public RedisProjectionBuilder ToBuilder()
        {
            return new RedisProjectionBuilder(this);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="RedisProjection"/> to <see><cref>RedisProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RedisProjectionHandler[](RedisProjection instance)
        {
            return instance.Handlers;
        }
    }
}