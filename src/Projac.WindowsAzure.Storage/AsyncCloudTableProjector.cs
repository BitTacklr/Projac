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
        private readonly Dictionary<Type, Node> _handlers;

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
        public Task ProjectAsync(CloudTableClient client, object message, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (message == null) throw new ArgumentNullException("message");

            Node node;
            if (_handlers.TryGetValue(message.GetType(), out node) && node != null)
            {
                return node.
                    Handler.Handler(client, message, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessageAsyncContinuation(node.Next, client, message, cancellationToken), 
                        cancellationToken,
                        TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
                        TaskScheduler.Current).
                    Unwrap();
            }
            return Task.FromResult(false);
        }

        private static Task ProjectMessageAsyncContinuation(Node node, CloudTableClient client, object message, CancellationToken cancellationToken)
        {
            if (node != null)
            {
                return node.
                    Handler.Handler(client, message, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessageAsyncContinuation(node.Next, client, message, cancellationToken),
                        cancellationToken,
                        TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.NotOnFaulted,
                        TaskScheduler.Current);
            }
            return Task.FromResult(false);
        }

        /// <summary>
        /// Projects the specified messages to project.
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
        public Task ProjectAsync(CloudTableClient client, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (messages == null) throw new ArgumentNullException("messages");

            var enumerator = messages.GetEnumerator();
            Node node;
            if (enumerator.MoveNext() && _handlers.TryGetValue(enumerator.Current.GetType(), out node) && node != null)
            {
                return node.
                    Handler.Handler(client, enumerator.Current, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessagesAsyncContinuation(node.Next, client, enumerator, _handlers, cancellationToken),
                        cancellationToken,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.Current);
            }
            enumerator.Dispose();
            return Task.FromResult(false);
        }

        private static Task ProjectMessagesAsyncContinuation(Node node, CloudTableClient client, IEnumerator<object> enumerator, Dictionary<Type, Node> handlers, CancellationToken cancellationToken)
        {
            if (node != null)
            {
                return node.
                    Handler.Handler(client, enumerator.Current, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessagesAsyncContinuation(node.Next, client, enumerator, handlers, cancellationToken),
                        cancellationToken,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.Current);
            }
            if (enumerator.MoveNext() && handlers.TryGetValue(enumerator.Current.GetType(), out node) && node != null)
            {
                return node.
                    Handler.Handler(client, enumerator.Current, cancellationToken).
                    ContinueWith(
                        _ => ProjectMessagesAsyncContinuation(node.Next, client, enumerator, handlers, cancellationToken),
                        cancellationToken,
                        TaskContinuationOptions.OnlyOnRanToCompletion,
                        TaskScheduler.Current);
            }
            enumerator.Dispose();
            return Task.FromResult(false);
        }

        private static Node ToLinkedList(IEnumerable<CloudTableProjectionHandler> handlers)
        {
            using (var enumerator = handlers.GetEnumerator())
            {
                Node node = null;
                while (enumerator.MoveNext())
                {
                    node = new Node(enumerator.Current, node);
                }
                return node;
            }
        }

        private class Node
        {
            public readonly CloudTableProjectionHandler Handler;
            public readonly Node Next;

            public Node(CloudTableProjectionHandler handler, Node next)
            {
                Handler = handler;
                Next = next;
            }
        }
    }
}
