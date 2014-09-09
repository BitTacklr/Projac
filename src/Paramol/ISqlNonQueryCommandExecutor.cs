using System.Collections.Generic;

namespace Paramol
{
    /// <summary>
    ///     Represents the synchronous execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data
    ///     source.
    /// </summary>
    public interface ISqlNonQueryCommandExecutor
    {
        /// <summary>
        ///     Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        int Execute(IEnumerable<SqlNonQueryCommand> commands);
    }
}