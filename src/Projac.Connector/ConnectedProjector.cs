using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector
{
    /// <summary>
    /// Projects a single message or set of messages in an asynchronous manner to the matching handlers.
    /// </summary>
    public class ConnectedProjector<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedProjector{TConnection}"/> class.
        /// </summary>
        /// <param name="resolver">The handler resolver.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="resolver"/> is <c>null</c>.</exception>
        public ConnectedProjector(ConnectedProjectionHandlerResolver<TConnection> resolver)
        {
            if (resolver == null) 
                throw new ArgumentNullException("resolver");

            _resolver = resolver;
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, object message)
        {
            return ProjectAsync(connection, message, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified message asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="message">The message to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="message"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, object message, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException("message");

            return 
                (
                    from handler in _resolver(message)
                    select handler.Handler(connection, message, cancellationToken)
                ).ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Projects the specified messages to project.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, IEnumerable<object> messages)
        {
            return ProjectAsync(connection, messages, CancellationToken.None);
        }

        /// <summary>
        /// Projects the specified messages asynchronously.
        /// </summary>
        /// <param name="connection">The connection used during projection.</param>
        /// <param name="messages">The messages to project.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" />.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> or <paramref name="messages"/> is <c>null</c>.</exception>
        public Task ProjectAsync(TConnection connection, IEnumerable<object> messages, CancellationToken cancellationToken)
        {
            if (messages == null) throw new ArgumentNullException("messages");

            return
                (
                    from message in messages
                    from handler in _resolver(message)
                    select handler.Handler(connection, message, cancellationToken)
                ).ExecuteAsync(cancellationToken);
        }
    }
}