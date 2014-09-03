using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol
{
    /// <summary>
    /// Represents the execution of a set of <see cref="SqlNonQueryStatement">statements</see> against a data source.
    /// </summary>
    public class TransactionalAsyncSqlNonQueryStatementExecutor : IAsyncSqlNonQueryStatementExecutor
    {
        private readonly ConnectionStringSettings _settings;
        private readonly IsolationLevel _isolationLevel;
        private readonly DbProviderFactory _dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalAsyncSqlNonQueryStatementExecutor"/> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <exception cref="System.ArgumentNullException">Throws when <paramref name="settings"/> is <c>null</c>.</exception>
        public TransactionalAsyncSqlNonQueryStatementExecutor(ConnectionStringSettings settings, IsolationLevel isolationLevel)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _isolationLevel = isolationLevel;
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
        }

        /// <summary>
        /// Executes the specified statements asynchronously.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>A <see cref="Task"/> that will return the number of <see cref="SqlNonQueryStatement">statements</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when <paramref name="statements"/> are <c>null</c>.</exception>
        public Task<int> ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements)
        {
            return ExecuteAsync(statements, CancellationToken.None);
        }

        /// <summary>
        /// Executes the specified statements asynchronously.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> that will return the number of <see cref="SqlNonQueryStatement">statements</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Throws when <paramref name="statements" /> are <c>null</c>.</exception>
        public async Task<int> ExecuteAsync(IEnumerable<SqlNonQueryStatement> statements, CancellationToken cancellationToken)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            using (var connection = _dbProviderFactory.CreateConnection())
            {
                connection.ConnectionString = _settings.ConnectionString;
                await connection.OpenAsync(cancellationToken);
                try
                {
                    var count = 0;
                    using (var transaction = connection.BeginTransaction(_isolationLevel))
                    {
                        using (var command = _dbProviderFactory.CreateCommand())
                        {
                            command.Connection = connection;
                            command.CommandType = CommandType.Text;
                            
                            foreach (var statement in statements)
                            {
                                command.CommandText = statement.Text;
                                command.Parameters.Clear();
                                command.Parameters.AddRange(statement.Parameters);
                                await command.ExecuteNonQueryAsync(cancellationToken);
                                count++;
                            }
                        }
                        transaction.Commit();
                    }
                    return count;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}