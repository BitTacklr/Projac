using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;

namespace Projac.RavenDB
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class AsyncRavenProjector
    {
        private readonly Dictionary<Type, RavenProjectionHandler[]> _handlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncRavenProjector"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> is <c>null</c>.</exception>
        public AsyncRavenProjector(RavenProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="session">The Raven session used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="session"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(IAsyncDocumentSession session, object message)
        {
            return ProjectAsync(session, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="session">The Raven session used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="session"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public async Task ProjectAsync(IAsyncDocumentSession session, object message, CancellationToken cancellationToken)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (message == null) throw new ArgumentNullException("message");

            RavenProjectionHandler[] handlers;
            if (_handlers.TryGetValue(message.GetType(), out handlers))
            {
                foreach (var handler in handlers)
                {
                    await handler.Handler(session, message, cancellationToken);
                }
            }
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="session">The Raven session used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="session"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public Task ProjectAsync(IAsyncDocumentSession session, IEnumerable<object> messages)
        {
            return ProjectAsync(session, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="session">The Raven session used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="session"/> or <paramref name="messages"/> are <c>null</c>.</exception>
        public async Task ProjectAsync(IAsyncDocumentSession session, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (session == null) throw new ArgumentNullException("session");
            if (messages == null) throw new ArgumentNullException("messages");

            foreach (var message in messages)
            {
                RavenProjectionHandler[] handlers;
                if (!_handlers.TryGetValue(message.GetType(), out handlers))
                    continue;

                foreach (var handler in handlers)
                {
                    await handler.Handler(session, message, cancellationToken);
                }
            }
        }
    }
}