using System;

namespace Projac.Testing
{
    /// <summary>
    /// The when state within the test specification building process.
    /// </summary>
    public interface ITSqlProjectionScenarioWhenStateBuilder
    {
        /// <summary>
        /// Expect the query to return the row count as a scalar.
        /// </summary>
        /// <param name="query">The count query to execute.</param>
        /// <param name="rowCount">The expected row count.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioExpectStateBuilder ExpectRowCount(TSqlQueryStatement query, int rowCount);

        /// <summary>
        /// Expect the query to return an empty resultset.
        /// </summary>
        /// <param name="query">The count query to execute.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioExpectStateBuilder ExpectEmptyResultSet(TSqlQueryStatement query);

        /// <summary>
        /// Expect the query to return a non empty resultset.
        /// </summary>
        /// <param name="query">The count query to execute.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioExpectStateBuilder ExpectNonEmptyResultSet(TSqlQueryStatement query);

        /// <summary>
        /// Expect the query to return the specified scalar.
        /// </summary>
        /// <param name="query">The count query to execute.</param>
        /// <param name="value">The expected scalar value.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioExpectStateBuilder ExpectScalar<TScalar>(TSqlQueryStatement query, TScalar value) where TScalar : IEquatable<TScalar>;
    }
}