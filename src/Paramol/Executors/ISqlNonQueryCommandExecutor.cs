using System.Collections.Generic;

namespace Paramol.Executors
{
    /// <summary>
    ///     Represents the synchronous execution of <see cref="SqlNonQueryCommand">commands</see> against a data
    ///     source.
    /// </summary>
    public interface ISqlNonQueryCommandExecutor
    {
        /// <summary>
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        void ExecuteNonQuery(SqlNonQueryCommand command);

        /// <summary>
        ///     Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        int ExecuteNonQuery(IEnumerable<SqlNonQueryCommand> commands);
    }
}