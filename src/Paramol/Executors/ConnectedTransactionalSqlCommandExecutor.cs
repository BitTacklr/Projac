using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol.Executors
{
    /// <summary>
    ///     Represents the execution of commands against a data source using a begun transaction.
    /// </summary>
    public class ConnectedTransactionalSqlCommandExecutor : 
        ISqlNonQueryCommandExecutor,
        IAsyncSqlNonQueryCommandExecutor
    {
        private readonly DbTransaction _dbTransaction;
        private readonly int _commandTimeout;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedTransactionalSqlCommandExecutor" /> class.
        /// </summary>
        /// <param name="dbTransaction">The transaction to execute the commands on.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dbTransaction" /> is <c>null</c>.</exception>
        public ConnectedTransactionalSqlCommandExecutor(DbTransaction dbTransaction, int commandTimeout = 30)
        {
            if (dbTransaction == null) throw new ArgumentNullException("dbTransaction");
            _dbTransaction = dbTransaction;
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

            using (var dbCommand = _dbTransaction.Connection.CreateCommand())
            {
                dbCommand.Connection = _dbTransaction.Connection;
                dbCommand.Transaction = _dbTransaction;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.Clear();
                dbCommand.Parameters.AddRange(command.Parameters);
                dbCommand.ExecuteNonQuery();
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

            var count = 0;
            using (var dbCommand = _dbTransaction.Connection.CreateCommand())
            {
                dbCommand.Connection = _dbTransaction.Connection;
                dbCommand.Transaction = _dbTransaction;
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
            return count;
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

            var count = 0;
            using (var dbCommand = _dbTransaction.Connection.CreateCommand())
            {
                dbCommand.Connection = _dbTransaction.Connection;
                dbCommand.Transaction = _dbTransaction;
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
            return count;
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

            using (var dbCommand = _dbTransaction.Connection.CreateCommand())
            {
                dbCommand.Connection = _dbTransaction.Connection;
                dbCommand.Transaction = _dbTransaction;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                await dbCommand.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}