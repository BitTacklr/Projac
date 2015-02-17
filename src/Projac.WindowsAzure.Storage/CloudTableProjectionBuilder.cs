using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Projac.WindowsAzure.Storage
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="CloudTableProjection" />.
    /// </summary>
    public class CloudTableProjectionBuilder
    {
        private readonly CloudTableProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloudTableProjectionBuilder" /> class.
        /// </summary>
        public CloudTableProjectionBuilder() :
            this(new CloudTableProjectionHandler[0])
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CloudTableProjectionBuilder" /> class.
        /// </summary>
        public CloudTableProjectionBuilder(CloudTableProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _handlers = projection.Handlers;
        }

        CloudTableProjectionBuilder(CloudTableProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <returns>A <see cref="CloudTableProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public CloudTableProjectionBuilder When<TMessage>(Func<CloudTableClient, TMessage, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new CloudTableProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new CloudTableProjectionHandler(
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
        /// <returns>A <see cref="CloudTableProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public CloudTableProjectionBuilder When<TMessage>(Func<CloudTableClient, TMessage, CancellationToken, Task> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new CloudTableProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new CloudTableProjectionHandler(
                            typeof (TMessage),
                            (client, message, token) => handler(client, (TMessage) message, token))
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Builds a projection specification based on the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="CloudTableProjection" />.</returns>
        public CloudTableProjection Build()
        {
            return new CloudTableProjection(_handlers);
        }
    }
}