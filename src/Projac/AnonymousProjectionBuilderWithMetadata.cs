using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    /// <summary>
    ///     Represents a fluent syntax to build up a set of <see cref="ProjectionHandler{TConnection, TMetadata}" />.
    /// </summary>
    public partial class AnonymousProjectionBuilder<TConnection, TMetadata>
    {
        private readonly ProjectionHandler<TConnection, TMetadata>[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousProjectionBuilder{TConnection, TMetadata}" /> class.
        /// </summary>
        public AnonymousProjectionBuilder() :
            this(new ProjectionHandler<TConnection, TMetadata>[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AnonymousProjectionBuilder{TConnection, TMetadata}" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="handlers"/> is <c>null</c>.</exception>
        public AnonymousProjectionBuilder(ProjectionHandler<TConnection, TMetadata>[] handlers)
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
        public AnonymousProjectionBuilder<TConnection, TMetadata> Handle<TMessage>(Func<TConnection, TMessage, TMetadata, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousProjectionBuilder<TConnection, TMetadata>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection, TMetadata>(
                            typeof (TMessage),
                            (connection, message, metadata, token) => handler(connection, (TMessage) message, metadata))
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
        public AnonymousProjectionBuilder<TConnection, TMetadata> Handle<TMessage>(Action<TConnection, TMessage, TMetadata> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            return new AnonymousProjectionBuilder<TConnection, TMetadata>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection, TMetadata>(
                            typeof (TMessage),
                            (connection, message, metadata, token) =>
                            {
                                handler(connection, (TMessage) message, metadata);
                                return Task.CompletedTask;
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
        public AnonymousProjectionBuilder<TConnection, TMetadata> Handle<TMessage>(Func<TConnection, TMessage, TMetadata, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new AnonymousProjectionBuilder<TConnection, TMetadata>(
                _handlers.Concat(
                    new[]
                    {
                        new ProjectionHandler<TConnection, TMetadata>(
                            typeof (TMessage),
                            (connection, message, metadata, token) => handler(connection, (TMessage) message, metadata, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds an <see cref="AnonymousProjection{TConnection}"/> using the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="AnonymousProjection{TConnection}" /> array.</returns>
        public AnonymousProjection<TConnection, TMetadata> Build()
        {
            return new AnonymousProjection<TConnection, TMetadata>(_handlers);
        }
    }
}