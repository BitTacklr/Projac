using System;
using System.Collections;
using System.Collections.Generic;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represent a SQL projection.
    /// </summary>
    public abstract class SqlProjection : IEnumerable<SqlProjectionHandler>
    {
        private readonly List<SqlProjectionHandler> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjection" /> class.
        /// </summary>
        protected SqlProjection()
        {
            _handlers = new List<SqlProjectionHandler>();
        }

        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Func<TMessage, SqlNonQueryCommand> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(new SqlProjectionHandler(typeof (TMessage), message => new[] {handler((TMessage) message)}));
        }

        /// <summary>
        ///     Specifies the non query command array returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Func<TMessage, SqlNonQueryCommand[]> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(new SqlProjectionHandler(typeof (TMessage), message => handler((TMessage) message)));
        }

        /// <summary>
        ///     Specifies the non query command enumeration returning handler to be invoked when a particular message occurs.
        /// </summary>
        /// <typeparam name="TMessage">The type of the message.</typeparam>
        /// <param name="handler">The command enumeration returning handler.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        protected void When<TMessage>(Func<TMessage, IEnumerable<SqlNonQueryCommand>> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handlers.Add(new SqlProjectionHandler(typeof(TMessage), message => handler((TMessage)message)));
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this projection.
        /// </value>
        public SqlProjectionHandler[] Handlers
        {
            get { return _handlers.ToArray(); }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SqlProjection"/> to <see><cref>SqlProjectionHandler[]</cref></see>.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator SqlProjectionHandler[](SqlProjection instance)
        {
            return instance.Handlers;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a copy of the handlers.
        /// </summary>
        /// <returns>
        /// An <see cref="SqlProjectionHandlerEnumerator" /> that can be used to iterate through a copy of the handlers.
        /// </returns>
        public SqlProjectionHandlerEnumerator GetEnumerator()
        {
            return new SqlProjectionHandlerEnumerator(Handlers);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An <see cref="SqlProjectionHandlerEnumerator" /> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<SqlProjectionHandler> IEnumerable<SqlProjectionHandler>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}