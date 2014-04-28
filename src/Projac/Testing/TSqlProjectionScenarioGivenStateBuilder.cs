using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    internal class TSqlProjectionScenarioGivenStateBuilder : ITSqlProjectionScenarioGivenStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;

        public TSqlProjectionScenarioGivenStateBuilder(TSqlProjection projection, object[] givens)
        {
            _projection = projection;
            _givens = givens;
        }

        public ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, _givens.Concat(events).ToArray());
        }

        public ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, _givens.Concat(events).ToArray());
        }

        public ITSqlProjectionScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new TSqlProjectionScenarioWhenStateBuilder(_projection, _givens, @event);
        }
    }
}