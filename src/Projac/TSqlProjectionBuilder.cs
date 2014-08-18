using System;
using System.Collections.Generic;
using System.Linq;
using TSqlClient;

namespace Projac
{
    /// <summary>
    ///     Represents a fluent syntax to build up a <see cref="TSqlProjection" />.
    /// </summary>
    public class TSqlProjectionBuilder
    {
        private readonly TSqlProjectionHandler[] _handlers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlProjectionBuilder" /> class.
        /// </summary>
        public TSqlProjectionBuilder() :
            this(new TSqlProjectionHandler[0])
        {
        }

        private TSqlProjectionBuilder(TSqlProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        /// <summary>
        ///     Specifies the single statement returning handler to be invoked when a particular event occurs.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The single statement returning handler.</param>
        /// <returns>A <see cref="TSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, TSqlNonQueryStatement> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new TSqlProjectionHandler
                            (
                            typeof (TEvent),
                            @event => new[] {handler((TEvent) @event)}
                            )
                    }).
                    ToArray());
        }

        /// <summary>
        ///     Specifies the statement array returning handler to be invoked when a particular event occurs.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="handler">The statement array returning handler.</param>
        /// <returns>A <see cref="TSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, TSqlNonQueryStatement[]> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new TSqlProjectionHandler
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
        /// <param name="handler">The non query statement enumeration returning handler.</param>
        /// <returns>A <see cref="TSqlProjectionBuilder" />.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="handler" /> is <c>null</c>.</exception>
        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, IEnumerable<TSqlNonQueryStatement>> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                _handlers.Concat(
                    new[]
                    {
                        new TSqlProjectionHandler
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
        /// <returns>A <see cref="TSqlProjection" />.</returns>
        public TSqlProjection Build()
        {
            return new TSqlProjection(_handlers);
        }
    }
}