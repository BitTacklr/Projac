using System;
using System.Collections;
using System.Collections.Generic;

namespace Projac.Sql
{
    /// <summary>
    ///     Represent a SQL projection.
    /// </summary>
    public abstract partial class SqlProjection : IEnumerable<SqlProjectionHandler>
    {
        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Func<TMessage, SqlNonQueryCommand> handler)
        {
            Handle(handler);
        }

        /// <summary>
        ///     Specifies the non query command array returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Func<TMessage, SqlNonQueryCommand[]> handler)
        {
            Handle(handler);
        }

        /// <summary>
        ///     Specifies the non query command enumeration returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command enumeration returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        [Obsolete("Please use the Handle method instead. This method will be removed in a future release.")]
        protected void When<TMessage>(Func<TMessage, IEnumerable<SqlNonQueryCommand>> handler)
        {
            Handle(handler);
        }
    }
}