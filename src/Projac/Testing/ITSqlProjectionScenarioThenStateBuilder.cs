namespace Projac.Testing
{
    public interface ITSqlProjectionScenarioThenStateBuilder : ITSqlProjectionTestSpecificationBuilder
    {
        ITSqlProjectionScenarioThenStateBuilder ThenCount(TSqlQueryStatement query, int count);
    }
}