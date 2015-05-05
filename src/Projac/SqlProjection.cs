using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represent a projection.
    /// </summary>
    public class SqlProjection
    {
        /// <summary>
        /// Returns a <see cref="SqlProjection"/> instance without handlers.
        /// </summary>
        public static readonly SqlProjection Empty = new SqlProjection(new SqlProjectionHandler[0]);

        private readonly SqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjection" /> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers" /> are <c>null</c>.</exception>
        public SqlProjection(SqlProjectionHandler[] handlers)
        {
            if (handlers == null) throw new ArgumentNullException("handlers");
            _handlers = handlers;
        }

        /// <summary>
        ///     Gets a read only collection of projection handlers.
        /// </summary>
        /// <value>
        ///     The projection handlers associated with this specification.
        /// </value>
        public SqlProjectionHandler[] Handlers
        {
            get { return _handlers; }
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the handlers of the specified projection.
        /// </summary>
        /// <param name="projection">The projection to concatenate.</param>
        /// <returns>A <see cref="SqlProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public SqlProjection Concat(SqlProjection projection)
        {
            if (projection == null) 
                throw new ArgumentNullException("projection");
            return Concat(projection.Handlers);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handler.
        /// </summary>
        /// <param name="handler">The projection handler to concatenate.</param>
        /// <returns>A <see cref="SqlProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler"/> is <c>null</c>.</exception>
        public SqlProjection Concat(SqlProjectionHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

            var concatenated = new SqlProjectionHandler[Handlers.Length + 1];
            Handlers.CopyTo(concatenated, 0);
            concatenated[Handlers.Length] = handler;
            return new SqlProjection(concatenated);
        }

        /// <summary>
        ///     Concatenates the handlers of this projection with the specified projection handlers.
        /// </summary>
        /// <param name="handlers">The projection handlers to concatenate.</param>
        /// <returns>A <see cref="SqlProjection"/> containing the concatenated handlers.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handlers"/> are <c>null</c>.</exception>
        public SqlProjection Concat(SqlProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");

            var concatenated = new SqlProjectionHandler[Handlers.Length + handlers.Length];
            Handlers.CopyTo(concatenated, 0);
            handlers.CopyTo(concatenated, Handlers.Length);
            return new SqlProjection(concatenated);
        }

        /// <summary>
        /// Creates a <see cref="SqlProjectionBuilder"/> based on the handlers of this projection.
        /// </summary>
        /// <returns>A <see cref="SqlProjectionBuilder"/>.</returns>
        public SqlProjectionBuilder ToBuilder()
        {
            return new SqlProjectionBuilder(this);
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
    }

    /// <summary>
    ///     Represent a SQL projection.
    /// </summary>
    public abstract class SqlProjection2 : IEnumerable<SqlProjectionHandler>
    {
        private readonly List<SqlProjectionHandler> _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjection" /> class.
        /// </summary>
        protected SqlProjection2()
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
        public static implicit operator SqlProjectionHandler[](SqlProjection2 instance)
        {
            return instance.Handlers;
        }

        public IEnumerator<SqlProjectionHandler> GetEnumerator()
        {
            return Handlers.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}