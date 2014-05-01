using System;

namespace Projac.Testing
{
    internal class ScenarioGivenNoneStateBuilder : IScenarioGivenNoneStateBuilder
    {
        private readonly TSqlProjection _projection;

        public ScenarioGivenNoneStateBuilder(TSqlProjection projection)
        {
            _projection = projection;
        }

        public IScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new ScenarioWhenStateBuilder(_projection, new object[0], @event);
        }
    }
}