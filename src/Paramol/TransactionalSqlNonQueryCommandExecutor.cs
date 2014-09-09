using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represents the execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data source.
    /// </summary>
    public class TransactionalSqlNonQueryCommandExecutor : ISqlNonQueryCommandExecutor
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly IsolationLevel _isolationLevel;
        private readonly ConnectionStringSettings _settings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionalSqlNonQueryCommandExecutor" /> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="settings" /> is <c>null</c>.</exception>
        public TransactionalSqlNonQueryCommandExecutor(ConnectionStringSettings settings,
            IsolationLevel isolationLevel)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _isolationLevel = isolationLevel;
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
                    var count = 0;
                    using (var dbTransaction = dbConnection.BeginTransaction(_isolationLevel))
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.Transaction = dbTransaction;

                            foreach (var command in commands)
                            {
                                dbCommand.CommandType = command.Type;
                                dbCommand.CommandText = command.Text;
                                dbCommand.Parameters.Clear();
                                dbCommand.Parameters.AddRange(command.Parameters);
                                dbCommand.ExecuteNonQuery();
                                count++;
                            }
                        }
                        dbTransaction.Commit();
                    }
                    return count;
                }
                finally
                {
                    dbConnection.Close();
                }
            }
        }
    }
}