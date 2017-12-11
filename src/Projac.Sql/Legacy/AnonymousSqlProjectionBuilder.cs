using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Sql
{
    /// <summary>
    ///     Represents a fluent syntax to build up an <see cref="AnonymousSqlProjection"/>.
    /// </summary>
    public partial class AnonymousSqlProjectionBuilder
    {
        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <returns>A <see cref="AnonymousSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousSqlProjectionBuilder When<TMessage>(Func<TMessage, SqlNonQueryCommand> handler)
        {
            return Handle(handler);
        }

        /// <summary>
        ///     Specifies the non query command array returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <returns>A <see cref="AnonymousSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousSqlProjectionBuilder When<TMessage>(Func<TMessage, SqlNonQueryCommand[]> handler)
        {
            return Handle(handler);
        }

        /// <summary>
        ///     Specifies the non query statement enumeration returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The non query command enumeration returning handler.</param>
        /// <returns>A <see cref="AnonymousSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        public AnonymousSqlProjectionBuilder When<TMessage>(Func<TMessage, IEnumerable<SqlNonQueryCommand>> handler)
        {
            return Handle(handler);
        }
    }
}