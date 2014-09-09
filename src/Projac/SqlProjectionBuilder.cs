using System;
using System.Collections.Generic;
using System.Linq;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="SqlProjection" />.
    /// </summary>
    public class SqlProjectionBuilder
    {
        private readonly SqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjectionBuilder" /> class.
        /// </summary>
        public SqlProjectionBuilder() :
            this(new SqlProjectionHandler[0])
        {
        }

        private SqlProjectionBuilder(SqlProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the single non query command returning handler to be invoked when a particular event occurs.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The single command returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TEvent>(Func<TEvent, SqlNonQueryCommand> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TEvent),
                            @event => new[] {handler((TEvent) @event)}
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the non query command array returning handler to be invoked when a particular event occurs.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The command array returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TEvent>(Func<TEvent, SqlNonQueryCommand[]> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TEvent),
                            @event => handler((TEvent) @event)
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the non query statement enumeration returning handler to be invoked when a particular event occurs.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The non query command enumeration returning handler.</param>
        /// <returns>A <see cref="SqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public SqlProjectionBuilder When<TEvent>(Func<TEvent, IEnumerable<SqlNonQueryCommand>> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new SqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new SqlProjectionHandler
                            (
                            typeof (TEvent),
                            @event => handler((TEvent) @event)
                            )
                    }).
                    ToArray());
        }


        /// <summary>
        ///     Builds a projection specification based on the handlers collected by this builder.
        /// </summary>
        /// <returns>A <see cref="SqlProjection" />.</returns>
        public SqlProjection Build()
        {
            return new SqlProjection(_handlers);
        }
    }
}