using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represents the execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data source.
    /// </summary>
    public class SqlNonQueryCommandExecutor : ISqlNonQueryCommandExecutor
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly ConnectionStringSettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryCommandExecutor" /> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="settings" /> is <c>null</c>.</exception>
        public SqlNonQueryCommandExecutor(ConnectionStringSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
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

            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _settings.ConnectionString;
                dbConnection.Open();
                try
                {
                    using (var dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.Connection = dbConnection;

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
                finally
                {
                    dbConnection.Close();
                }
            }
        }
    }
}