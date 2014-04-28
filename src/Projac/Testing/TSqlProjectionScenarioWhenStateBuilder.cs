using System;

namespace Projac.Testing
{
    internal class TSqlProjectionScenarioWhenStateBuilder : ITSqlProjectionScenarioWhenStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;

        public TSqlProjectionScenarioWhenStateBuilder(TSqlProjection projection, object[] givens, object when)
        {
            _projection = projection;
            _givens = givens;
            _when = when;
        }

        public ITSqlProjectionScenarioThenStateBuilder ThenCount(TSqlQueryStatement query, int count)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioThenStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionVerification[] { new TSqlProjectionCountVerification(query, count) });
        }
    }
}