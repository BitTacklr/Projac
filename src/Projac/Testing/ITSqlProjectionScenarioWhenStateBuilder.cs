namespace Projac.Testing
{
    public interface ITSqlProjectionScenarioWhenStateBuilder
    {
        ITSqlProjectionScenarioThenStateBuilder ThenCount(TSqlQueryStatement query, int count);
    }
}