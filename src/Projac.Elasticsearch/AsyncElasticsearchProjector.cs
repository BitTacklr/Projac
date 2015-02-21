using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Projac.Elasticsearch
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncElasticsearchProjector
    {
        private readonly Dictionary<Type, ElasticsearchProjectionHandler[]> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncElasticsearchProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> is <c>null</c>.</exception>
        public AsyncElasticsearchProjector(ElasticsearchProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The Elasticsearch client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(IElasticsearchClient client, object message)
        {
            return ProjectAsync(client, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The Elasticsearch client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public async Task ProjectAsync(IElasticsearchClient client, object message, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (message == null) throw new ArgumentNullException("message");

            ElasticsearchProjectionHandler[] handlers;
            if (_handlers.TryGetValue(message.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    await handler.Handler(client, message, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="client">The Elasticsearch client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(IElasticsearchClient client, IEnumerable<object> messages)
        {
            return ProjectAsync(client, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="client">The Elasticsearch client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public async Task ProjectAsync(IElasticsearchClient client, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (messages == null) throw new ArgumentNullException("messages");

            foreach (var message in messages)
            {
                ElasticsearchProjectionHandler[] handlers;
                if (!_handlers.TryGetValue(message.GetType(), out handlers)) 
                    continue;

                foreach (var handler in handlers)
                {
                    await handler.Handler(client, message, cancellationToken);
                }
            }
        }
    }
}
