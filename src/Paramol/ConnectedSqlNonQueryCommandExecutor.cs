using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represents the execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data source using
    ///     an open connection.
    /// </summary>
    public class ConnectedSqlNonQueryCommandExecutor : ISqlNonQueryCommandExecutor
    {
        private readonly DbConnection _dbConnection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryCommandExecutor" /> class.
        /// </summary>
        /// <param name="dbConnection">The connection to execute the commands on.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dbConnection" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="dbConnection" /> is not open.</exception>
        public ConnectedSqlNonQueryCommandExecutor(DbConnection dbConnection)
        {
            if (dbConnection == null)
                throw new ArgumentNullException("dbConnection");
            if (dbConnection.State != ConnectionState.Open)
                throw new ArgumentException(
                    "The connection must be in the 'Open' state. Please make sure you've opened the connection beforehand.",
                    "dbConnection");
            _dbConnection = dbConnection;
        }

        /// <summary>
        ///     Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        public int Execute(IEnumerable<SqlNonQueryCommand> commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;

                var count = 0;
                foreach (var command in commands)
                {
                    dbCommand.CommandType = command.Type;
                    dbCommand.CommandText = command.Text;
                    dbCommand.Parameters.Clear();
                    dbCommand.Parameters.AddRange(command.Parameters);
                    dbCommand.ExecuteNonQuery();
                    count++;
                }
                return count;
            }
        }
    }
}