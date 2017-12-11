using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    public partial class AnonymousProjectionBuilder<TConnection>
    {
        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            return Handle<TMessage>(handler);
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message synchronously.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Action<TConnection, TMessage> handler)
        {
            return Handle<TMessage>(handler);
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler that handles the message asynchronously and with cancellation support.</param>
        /// <returns>A <see cref="AnonymousProjectionBuilder{TConnection}" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousProjectionBuilder<TConnection> When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            return Handle<TMessage>(handler);
        }
    }
}