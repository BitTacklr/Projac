using System;
using System.Collections.Generic;
#if !NETSTANDARD2_0
using System.Configuration;
#endif
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Sql.Executors
{
    /// <summary>
    ///     Represents the transactional execution of commands against a data source.
    /// </summary>
    public class TransactionalSqlCommandExecutor :
        ISqlNonQueryCommandExecutor,
        IAsyncSqlNonQueryCommandExecutor
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly IsolationLevel _isolationLevel;
        private readonly int _commandTimeout;
        private readonly string _connectionString;

#if !NETSTANDARD2_0
        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionalSqlCommandExecutor" /> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="settings" /> is <c>null</c>.</exception>
        public TransactionalSqlCommandExecutor(ConnectionStringSettings settings,
            IsolationLevel isolationLevel,
            int commandTimeout = 30)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
            _connectionString = settings.ConnectionString;
            _isolationLevel = isolationLevel;
            _commandTimeout = commandTimeout;
        }
#endif

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionalSqlCommandExecutor" /> class.
        /// </summary>
        /// <param name="dbProviderFactory">The database provider factory.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dbProviderFactory" /> or <paramref name="connectionString" /> is <c>null</c>.</exception>
        public TransactionalSqlCommandExecutor(DbProviderFactory dbProviderFactory, 
            string connectionString,
            IsolationLevel isolationLevel,
            int commandTimeout = 30)
        {
            if (dbProviderFactory == null) throw new ArgumentNullException("dbProviderFactory");
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _dbProviderFactory = dbProviderFactory;
            _connectionString = connectionString;
            _isolationLevel = isolationLevel;
            _commandTimeout = commandTimeout;
        }

        /// <summary>
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public void ExecuteNonQuery(SqlNonQueryCommand command)
        {
            if (command == null) 
                throw new ArgumentNullException("command");

            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                dbConnection.Open();
                try
                {
                    using (var dbTransaction = dbConnection.BeginTransaction(_isolationLevel))
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.Transaction = dbTransaction;
                            dbCommand.CommandTimeout = _commandTimeout;

                            dbCommand.CommandType = command.Type;
                            dbCommand.CommandText = command.Text;
                            dbCommand.Parameters.AddRange(command.Parameters);
                            dbCommand.ExecuteNonQuery();
                        }
                        dbTransaction.Commit();
                    }
                }
                finally
                {
                    dbConnection.Close();
                }
            }
        }

        /// <summary>
        ///     Executes the specified commands.
        /// </summary>
        /// <param name="commands">The commands to execute.</param>
        /// <returns>The number of <see cref="SqlNonQueryCommand">commands</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        public int ExecuteNonQuery(IEnumerable<SqlNonQueryCommand> commands)
        {
            if (commands == null) 
                throw new ArgumentNullException("commands");

            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
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
                            dbCommand.CommandTimeout = _commandTimeout;

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

        /// <summary>
        ///     Executes the specified commands asynchronously.
        /// </summary>
        /// <param name="commands">The commands</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return the number of <see cref="SqlNonQueryCommand">commands</see>
        ///     executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="commands" /> are <c>null</c>.</exception>
        public Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands)
        {
            return ExecuteNonQueryAsync(commands, CancellationToken.None);
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
        public async Task<int> ExecuteNonQueryAsync(IEnumerable<SqlNonQueryCommand> commands, CancellationToken cancellationToken)
        {
            if (commands == null) 
                throw new ArgumentNullException("commands");

            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                await dbConnection.OpenAsync(cancellationToken);
                try
                {
                    var count = 0;
                    using (var dbTransaction = dbConnection.BeginTransaction(_isolationLevel))
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.Transaction = dbTransaction;
                            dbCommand.CommandTimeout = _commandTimeout;

                            foreach (var command in commands)
                            {
                                dbCommand.CommandType = command.Type;
                                dbCommand.CommandText = command.Text;
                                dbCommand.Parameters.Clear();
                                dbCommand.Parameters.AddRange(command.Parameters);
                                await dbCommand.ExecuteNonQueryAsync(cancellationToken);
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

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return when the <see cref="SqlNonQueryCommand">command</see>
        ///     was executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public Task ExecuteNonQueryAsync(SqlNonQueryCommand command)
        {
            return ExecuteNonQueryAsync(command, CancellationToken.None);
        }

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return when the <see cref="SqlNonQueryCommand">command</see>
        ///     was executed.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public async Task ExecuteNonQueryAsync(SqlNonQueryCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            using (var dbConnection = _dbProviderFactory.CreateConnection())
            {
                dbConnection.ConnectionString = _connectionString;
                await dbConnection.OpenAsync(cancellationToken);
                try
                {
                    using (var dbTransaction = dbConnection.BeginTransaction(_isolationLevel))
                    {
                        using (var dbCommand = dbConnection.CreateCommand())
                        {
                            dbCommand.Connection = dbConnection;
                            dbCommand.Transaction = dbTransaction;
                            dbCommand.CommandTimeout = _commandTimeout;

                            dbCommand.CommandType = command.Type;
                            dbCommand.CommandText = command.Text;
                            dbCommand.Parameters.AddRange(command.Parameters);
                            await dbCommand.ExecuteNonQueryAsync(cancellationToken);
                        }
                        dbTransaction.Commit();
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