namespace Projac.Testing
{
    public interface ITSqlProjectionScenarioGivenNoneStateBuilder
    {
        ITSqlProjectionScenarioWhenStateBuilder When(object @event);
    }
}