using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public abstract partial class Projection<TConnection>
    {
        /// <summary>
        ///     Specifies the asynchronous message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Func<TConnection, TMessage, Task> handler)
        {
            Handle<TMessage>(handler);
        }

        /// <summary>
        ///     Specifies the synchronous message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Action<TConnection, TMessage> handler)
        {
            Handle<TMessage>(handler);
        }

        /// <summary>
        ///     Specifies the message handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The message handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Func<TConnection, TMessage, CancellationToken, Task> handler)
        {
            Handle<TMessage>(handler);
        }
    }
}