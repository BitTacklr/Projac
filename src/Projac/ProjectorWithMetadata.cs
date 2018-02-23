using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{

    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class Projector<TConnection, TMetadata>
    {
        private readonly ProjectionHandlerResolver<TConnection, TMetadata> _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projector{TConnection, TMetadata}"/> class.
        /// </summary>
        /// <param name="resolver">The handler resolver.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="resolver"/> is <c>null</c>.</exception>
        public Projector(ProjectionHandlerResolver<TConnection, TMetadata> resolver)
        {
            if (resolver == null) 
                throw new ArgumentNullException(nameof(resolver));

            _resolver = resolver;
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="metadata">The metadata associated with the message.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, object message, TMetadata metadata)
        {
            return ProjectAsync(connection, message, metadata, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="metadata">The metadata associated with the message.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, object message, TMetadata metadata, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            return 
                (
                    from handler in _resolver(message)
                    select handler.Handler(connection, message, metadata, cancellationToken)
                ).ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Projects the specified messages to project.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="messages">The messages with associated metadata to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, IEnumerable<(object message, TMetadata metadata)> messages)
        {
            return ProjectAsync(connection, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="messages">The messages with associated metadata to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, IEnumerable<(object message, TMetadata metadata)> messages, CancellationToken cancellationToken)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            return
                (
                    from item in messages
                    from handler in _resolver(item.message)
                    select handler.Handler(connection, item.message, item.metadata, cancellationToken)
                ).ExecuteAsync(cancellationToken);
        }
    }
}