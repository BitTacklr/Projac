using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Projac.WindowsAzure.Storage
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncCloudTableProjector
    {
        private readonly Dictionary<Type, HandlerNode> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCloudTableProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> is <c>null</c>.</exception>
        public AsyncCloudTableProjector(CloudTableProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, ToLinkedList);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, object message)
        {
            return ProjectAsync(client, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public async Task ProjectAsync(CloudTableClient client, object message, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (message == null) throw new ArgumentNullException("message");

            HandlerNode node;
            if (_handlers.TryGetValue(message.GetType(), out node) && node != null)
            {
                while (node != null)
                {
                    await node.Handler.Handler(client, message, cancellationToken);
                    node = node.Next;
                }
            }
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(CloudTableClient client, IEnumerable<object> messages)
        {
            return ProjectAsync(client, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="client">The cloud table client used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="client"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public async Task ProjectAsync(CloudTableClient client, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (messages == null) throw new ArgumentNullException("messages");

            foreach (var message in messages)
            {
                HandlerNode node;
                if (!_handlers.TryGetValue(message.GetType(), out node) || node == null) 
                    continue;

                while (node != null)
                {
                    await node.Handler.Handler(client, message, cancellationToken);
                    node = node.Next;
                }
            }
        }

        private static HandlerNode ToLinkedList(IEnumerable<CloudTableProjectionHandler> handlers)
        {
            using (var enumerator = handlers.GetEnumerator())
            {
                HandlerNode handlerNode = null;
                while (enumerator.MoveNext())
                {
                    handlerNode = new HandlerNode(enumerator.Current, handlerNode);
                }
                return handlerNode;
            }
        }

        private class HandlerNode
        {
            public readonly CloudTableProjectionHandler Handler;
            public readonly HandlerNode Next;

            public HandlerNode(CloudTableProjectionHandler handler, HandlerNode next)
            {
                Handler = handler;
                Next = next;
            }
        }
    }
}
