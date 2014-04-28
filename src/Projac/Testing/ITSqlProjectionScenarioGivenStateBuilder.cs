using System.Collections.Generic;

namespace Projac.Testing
{
    public interface ITSqlProjectionScenarioGivenStateBuilder
    {
        ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events);
        ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events);
        ITSqlProjectionScenarioWhenStateBuilder When(object @event);
    }
}