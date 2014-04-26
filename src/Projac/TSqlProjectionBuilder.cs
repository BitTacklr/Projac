using System;
using System.Collections.Generic;

namespace Projac
{
    public class TSqlProjectionBuilder
    {
        private readonly TSqlProjectionHandler[] _handlers;

        public TSqlProjectionBuilder() : 
            this(new TSqlProjectionHandler[0])
        {
        }

        private TSqlProjectionBuilder(TSqlProjectionHandler[] handlers)
        {
            _handlers = handlers;
        }

        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, ITSqlStatement> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                new[]
                {
                    new TSqlProjectionHandler
                        (
                            typeof (TEvent),
                            @event => new[] {handler((TEvent) @event)}
                        )
                });
        }


        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, ITSqlStatement[]> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                new[]
                {
                    new TSqlProjectionHandler
                        (
                            typeof (TEvent),
                            @event => handler((TEvent) @event)
                        )
                });
        }

        public TSqlProjectionBuilder When<TEvent>(Func<TEvent, IEnumerable<ITSqlStatement>> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            return new TSqlProjectionBuilder(
                new[]
                {
                    new TSqlProjectionHandler
                        (
                            typeof (TEvent),
                            @event => handler((TEvent) @event)
                        )
                });
        }


        public TSqlProjectionSpecification Build()
        {
            return new TSqlProjectionSpecification(_handlers);
        }
    }
}