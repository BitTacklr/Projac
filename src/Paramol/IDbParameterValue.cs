using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represents a database parameter value that can be converted to a <see cref="DbParameter" />.
    /// </summary>
    public interface IDbParameterValue
    {
        /// <summary>
        ///     Creates a <see cref="DbParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="DbParameter" />.</returns>
        DbParameter ToDbParameter(string parameterName);
    }
}