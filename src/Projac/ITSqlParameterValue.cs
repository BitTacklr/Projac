using System.Data.SqlClient;

namespace Projac
{
    /// <summary>
    ///     Represents a database parameter value that can be converted to a <see cref="SqlParameter" />.
    /// </summary>
    public interface ITSqlParameterValue
    {
        /// <summary>
        ///     Creates a <see cref="SqlParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="SqlParameter" />.</returns>
        SqlParameter ToSqlParameter(string parameterName);
    }
}