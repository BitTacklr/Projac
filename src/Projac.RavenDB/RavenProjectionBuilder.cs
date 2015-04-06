using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;

namespace Projac.RavenDB
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="RavenProjection" />.
    /// </summary>
    public class RavenProjectionBuilder
    {
        private readonly RavenProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RavenProjectionBuilder" /> class.
        /// </summary>
        public RavenProjectionBuilder() :
            this(new RavenProjectionHandler[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RavenProjectionBuilder" /> class.
        /// </summary>
        public RavenProjectionBuilder(RavenProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _handlers = projection.Handlers;
        }

        RavenProjectionBuilder(RavenProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="RavenProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public RavenProjectionBuilder When<TMessage>(Func<IAsyncDocumentSession, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new RavenProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new RavenProjectionHandler(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="RavenProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public RavenProjectionBuilder When<TMessage>(Func<IAsyncDocumentSession, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new RavenProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new RavenProjectionHandler(
                            typeof (TMessage),
                            (connection, message, token) => handler(connection, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds a projection specification based on the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="RavenProjection" />.</returns>
        public RavenProjection Build()
        {
            return new RavenProjection(_handlers);
        }
    }
}