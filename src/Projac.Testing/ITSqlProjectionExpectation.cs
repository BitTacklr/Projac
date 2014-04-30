using System.Data.SqlClient;

namespace Projac.Testing
{
    /// <summary>
    /// Embodies a specific expectation about the projection handling outcome.
    /// </summary>
    public interface ITSqlProjectionExpectation
    {
        /// <summary>
        /// Determines whether this expectation is satisfied.
        /// </summary>
        /// <param name="transaction">The transaction used to perform the expectation verification.</param>
        /// <returns><c>true</c> if the expectation is met, otherwise <c>false</c>.</returns>
        bool IsSatisfied(SqlTransaction transaction);
    }
}