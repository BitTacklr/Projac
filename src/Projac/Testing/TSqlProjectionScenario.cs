using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    public class TSqlProjectionScenario : ITSqlProjectionScenarioInitialStateBuilder
    {
        private readonly TSqlProjection _projection;

        public TSqlProjectionScenario(TSqlProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _projection = projection;
        }

        public ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, events);
        }

        public ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, events.ToArray());
        }

        public ITSqlProjectionScenarioGivenNoneStateBuilder GivenNone()
        {
            return new TSqlProjectionScenarioGivenNoneStateBuilder(_projection);
        }

        public ITSqlProjectionScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new TSqlProjectionScenarioWhenStateBuilder(_projection, new object[0], @event);
        }
    }
}
