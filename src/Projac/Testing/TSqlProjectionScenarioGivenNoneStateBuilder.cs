using System;

namespace Projac.Testing
{
    internal class TSqlProjectionScenarioGivenNoneStateBuilder : ITSqlProjectionScenarioGivenNoneStateBuilder
    {
        private readonly TSqlProjection _projection;

        public TSqlProjectionScenarioGivenNoneStateBuilder(TSqlProjection projection)
        {
            _projection = projection;
        }

        public ITSqlProjectionScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new TSqlProjectionScenarioWhenStateBuilder(_projection, new object[0], @event);
        }
    }
}