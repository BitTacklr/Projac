using System.Collections.Generic;

namespace Projac.Testing
{
    public interface ITSqlProjectionScenarioInitialStateBuilder
    {
        ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events);
        ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events);
        ITSqlProjectionScenarioGivenNoneStateBuilder GivenNone();
        ITSqlProjectionScenarioWhenStateBuilder When(object @event);
    }
}