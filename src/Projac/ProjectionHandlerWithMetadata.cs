using System;
using System.Threading;
using System.Threading.Tasks;

namespace Projac
{
    /// <summary>
    ///     Represents a handler of a particular type of message with the specified metadata.
    /// </summary>
    public class ProjectionHandler<TConnection, TMetadata>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProjectionHandler{TConnection}" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="message" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public ProjectionHandler(Type message, Func<TConnection, object, TMetadata, CancellationToken, Task> handler)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        ///     The type of message to handle.
        /// </summary>
        public Type Message { get; }

        /// <summary>
        ///     The function that handles the message.
        /// </summary>
        public Func<TConnection, object, TMetadata, CancellationToken, Task> Handler { get; }
    }
}
