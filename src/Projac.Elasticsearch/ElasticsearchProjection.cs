using System;

namespace Projac.Elasticsearch
{
    /// <summary>
    ///     Represent a Elasticsearch backed projection.
    /// </summary>
    public class ElasticsearchProjection
    {
        /// <summary>
        /// Returns a <see cref="ElasticsearchProjection"/> instance without handlers.
        /// </summary>
        public static readonly ElasticsearchProjection Empty = new ElasticsearchProjection(new ElasticsearchProjectionHandler[0]);

        private readonly ElasticsearchProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ElasticsearchProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public ElasticsearchProjection(ElasticsearchProjectionHandler[] handlers)
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
        public ElasticsearchProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="ElasticsearchProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public ElasticsearchProjection Concat(ElasticsearchProjection projection)
        {
            if (projection == null)
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="ElasticsearchProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public ElasticsearchProjection Concat(ElasticsearchProjectionHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new ElasticsearchProjectionHandler[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new ElasticsearchProjection(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="ElasticsearchProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public ElasticsearchProjection Concat(ElasticsearchProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new ElasticsearchProjectionHandler[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new ElasticsearchProjection(concatenated);
        }

        /// <summary>
        /// Creates a <see cref="ElasticsearchProjectionBuilder"/> based on the handlers of this projection.
        /// </summary>
        /// <returns>A <see cref="ElasticsearchProjectionBuilder"/>.</returns>
        public ElasticsearchProjectionBuilder ToBuilder()
        {
            return new ElasticsearchProjectionBuilder(this);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ElasticsearchProjection"/> to <see><cref>ElasticsearchProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ElasticsearchProjectionHandler[](ElasticsearchProjection instance)
        {
            return instance.Handlers;
        }
    }
}