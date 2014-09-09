using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol
{
    /// <summary>
    ///     Represents the execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data source.
    /// </summary>
    public class AsyncSqlNonQueryCommandExecutor : IAsyncSqlNonQueryCommandExecutor
    {
        private readonly int _commandTimeout;
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly ConnectionStringSettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncSqlNonQueryCommandExecutor" /> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="settings" /> is <c>null</c>.</exception>
        public AsyncSqlNonQueryCommandExecutor(ConnectionStringSettings settings, int commandTimeout = 30)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _commandTimeout = commandTimeout;
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
        }

        /// <summary>
        ///     Executes the specified commands asynchronously.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        public Task<int> ExecuteAsync(IEnumerable<SqlNonQueryCommand> commands)
        {
            return ExecuteAsync(commands, CancellationToken.None);
        }

        /// <summary>
        ///     Executes the specified commands asynchronously.
        /// </summary>
        /// <param name="commands">The commands.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        public async Task<int> ExecuteAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _settings.ConnectionString;
                await dbConnection.OpenAsync(cancellationToken);
                try
                {
                    using (var dbCommand = dbConnection.CreateCommand())
                    {
                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandTimeout = _commandTimeout;
                        var count = 0;
                        foreach (var command in commands)
                        {
                            dbCommand.CommandType = command.Type;
                            dbCommand.CommandText = command.Text;
                            dbCommand.Parameters.Clear();
                            dbCommand.Parameters.AddRange(command.Parameters);
                            await dbCommand.ExecuteNonQueryAsync(cancellationToken);
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