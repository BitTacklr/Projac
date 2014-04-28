using System;
using System.Linq;

namespace Projac.Testing
{
    internal class TSqlProjectionScenarioThenStateBuilder : ITSqlProjectionScenarioThenStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;
        private readonly ITSqlProjectionVerification[] _verifications;

        public TSqlProjectionScenarioThenStateBuilder(TSqlProjection projection, object[] givens, object when, ITSqlProjectionVerification[] verifications)
        {
            _projection = projection;
            _givens = givens;
            _when = when;
            _verifications = verifications;
        }

        public ITSqlProjectionScenarioThenStateBuilder ThenCount(TSqlQueryStatement query, int count)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new TSqlProjectionScenarioThenStateBuilder(
                _projection,
                _givens,
                _when,
                _verifications.Concat(new[] { new TSqlProjectionCountVerification(query, count) }).ToArray());
        }

        public TSqlProjectionTestSpecification Build()
        {
            return new TSqlProjectionTestSpecification(
                _projection,
                _givens,
                _when,
                _verifications);
        }
    }
}