using System;

namespace Projac.Testing
{
    internal class ScenarioWhenStateBuilder : IScenarioWhenStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;

        public ScenarioWhenStateBuilder(TSqlProjection projection, object[] givens, object when)
        {
            _projection = projection;
            _givens = givens;
            _when = when;
        }

        public IScenarioExpectStateBuilder ExpectRowCount(TSqlQueryStatement query, int rowCount)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new ScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new IExpectation[] { new RowCountExpectation(query, rowCount) });
        }

        public IScenarioExpectStateBuilder ExpectEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new ScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new IExpectation[] { new EmptyResultSetExpectation(query) });
        }

        public IScenarioExpectStateBuilder ExpectNonEmptyResultSet(TSqlQueryStatement query)
        {
            if (query == null) throw new ArgumentNullException("query");
            return new ScenarioExpectStateBuilder(
                _projection,
                _givens,
                _when,
                new IExpectation[] { new NonEmptyResultSetExpectation(query) });
        }
    }
}