using System;
using System.Linq;

namespace Projac.Testing
{
    internal class TSqlProjectionScenarioExpectStateBuilder : ITSqlProjectionScenarioExpectStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;
        private readonly ITSqlProjectionExpectation[] _expectations;

        public TSqlProjectionScenarioExpectStateBuilder(TSqlProjection projection, object[] givens, object when, ITSqlProjectionExpectation[] expectations)
        {
            _projection = projection;
            _givens = givens;
            _when = when;
            _expectations = expectations;
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectRowCount(TSqlQueryStatement query, int rowCount)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                _expectations.Concat(new[] { new TSqlProjectionRowCountExpectation(query, rowCount) }).ToArray());
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                _expectations.Concat(new[] { new TSqlProjectionEmptyResultSetExpectation(query)  }).ToArray());
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectNonEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                _expectations.Concat(new[] { new TSqlProjectionNonEmptyResultSetExpectation(query) }).ToArray());
        }

        public ITSqlProjectionScenarioExpectStateBuilder ExpectScalar<TScalar>(TSqlQueryStatement query, TScalar value) where TScalar : IEquatable<TScalar>
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                _expectations.Concat(new[] { new TSqlProjectionScalarExpectation<TScalar>(query, value) }).ToArray());
        }

        public TSqlProjectionTestSpecification Build()
        {
            return new TSqlProjectionTestSpecification(
                _projection,
                _givens,
                _when,
                _expectations);
        }
    }
}