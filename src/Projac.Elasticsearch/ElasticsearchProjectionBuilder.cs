using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Projac.Elasticsearch
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="ElasticsearchProjection" />.
    /// </summary>
    public class ElasticsearchProjectionBuilder
    {
        private readonly ElasticsearchProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ElasticsearchProjectionBuilder" /> class.
        /// </summary>
        public ElasticsearchProjectionBuilder() :
            this(new ElasticsearchProjectionHandler[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ElasticsearchProjectionBuilder" /> class.
        /// </summary>
        public ElasticsearchProjectionBuilder(ElasticsearchProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _handlers = projection.Handlers;
        }

        ElasticsearchProjectionBuilder(ElasticsearchProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="ElasticsearchProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public ElasticsearchProjectionBuilder When<TMessage>(Func<IElasticsearchClient, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new ElasticsearchProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new ElasticsearchProjectionHandler(
                            typeof (TMessage),
                            (client, message, token) => handler(client, (TMessage) message))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="ElasticsearchProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public ElasticsearchProjectionBuilder When<TMessage>(Func<IElasticsearchClient, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new ElasticsearchProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new ElasticsearchProjectionHandler(
                            typeof (TMessage),
                            (client, message, token) => handler(client, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds a projection specification based on the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="ElasticsearchProjection" />.</returns>
        public ElasticsearchProjection Build()
        {
            return new ElasticsearchProjection(_handlers);
        }
    }
}