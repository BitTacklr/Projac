using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    /// <summary>
    ///     Represents a fluent syntax to build up a set of <see cref="ProjectionHandler{TConnection}" />.
    /// </summary>
    public class AnonymousProjectionBuilder<TConnection>
    {
        private readonly ProjectionHandler<TConnection>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousProjectionBuilder{TConnection}" /> class.
        /// </summary>
        public AnonymousProjectionBuilder() :
            this(new ProjectionHandler<TConnection>[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousProjectionBuilder{TConnection}" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlers"/> is <c>null</c>.</exception>
        public AnonymousProjectionBuilder(ProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message synchronously.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Action<TConnection, TMessage> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            return new AnonymousProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) =>
                            {
                                handler(connection, (TMessage) message);
                                return Task.FromResult<object>(null);
                            })
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously and with cancellation support.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousProjectionBuilder<TConnection>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection>(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds an <see cref="AnonymousProjection{TConnection}"/> using the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="AnonymousProjection{TConnection}" /> array.</returns>
        public AnonymousProjection<TConnection> Build()
        {
            return new AnonymousProjection<TConnection>(_handlers);
        }
    }
}