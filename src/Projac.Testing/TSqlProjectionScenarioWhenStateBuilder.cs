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

        public ITSqlProjectionScenarioExpectStateBuilder ThenCount(TSqlQueryStatement query, int count)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionExpectation[] { new TSqlProjectionRowCountExpectation(query, count),  });
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectRowCount(TSqlQueryStatement query, int rowCount)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionExpectation[] { new TSqlProjectionRowCountExpectation(query, rowCount) });
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionExpectation[] { new TSqlProjectionEmptyResultSetExpectation(query) });
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectNonEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionExpectation[] { new TSqlProjectionNonEmptyResultSetExpectation(query) });
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectScalar<TScalar>(TSqlQueryStatement query, TScalar value) where TScalar : IEquatable<TScalar>
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new ITSqlProjectionExpectation[] { new TSqlProjectionScalarExpectation<TScalar>(query, value) });
        }
    }
}