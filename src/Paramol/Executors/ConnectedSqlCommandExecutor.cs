using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Paramol.Executors
{
    /// <summary>
    ///     Represents the execution of commands against a data source using an open connection.
    /// </summary>
    public class ConnectedSqlCommandExecutor :
        ISqlQueryCommandExecutor,
        ISqlNonQueryCommandExecutor,
        IAsyncSqlQueryCommandExecutor,
        IAsyncSqlNonQueryCommandExecutor
    {
        private readonly DbConnection _dbConnection;
        private readonly int _commandTimeout;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConnectedSqlCommandExecutor" /> class.
        /// </summary>
        /// <param name="dbConnection">The connection to execute the commands on.</param>
        /// <param name="commandTimeout">The command timeout.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dbConnection" /> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="dbConnection" /> is not open.</exception>
        public ConnectedSqlCommandExecutor(DbConnection dbConnection, int commandTimeout = 30)
        {
            if (dbConnection == null)
                throw new ArgumentNullException("dbConnection");
            if (dbConnection.State != ConnectionState.Open)
                throw new ArgumentException(
                    "The connection must be in the 'Open' state. Please make sure you've opened the connection beforehand.",
                    "dbConnection");
            _dbConnection = dbConnection;
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

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
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
            if (commands == null) throw new ArgumentNullException("commands");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

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

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                await dbCommand.ExecuteNonQueryAsync(cancellationToken);
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

            var count = 0;
            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
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
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="DbDataReader"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public DbDataReader ExecuteReader(SqlQueryCommand command)
        {
            if (command == null) 
                throw new ArgumentNullException("command");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                return dbCommand.ExecuteReader(CommandBehavior.SequentialAccess);
            }
        }

        /// <summary>
        ///     Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="object">scalar value</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public object ExecuteScalar(SqlQueryCommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                return dbCommand.ExecuteScalar();
            }
        }

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="object">scalar value</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public Task<object> ExecuteScalarAsync(SqlQueryCommand command)
        {
            return ExecuteScalarAsync(command, CancellationToken.None);
        }

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="object">scalar value</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public Task<object> ExecuteScalarAsync(SqlQueryCommand command, CancellationToken cancellationToken)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                return dbCommand.ExecuteScalarAsync(cancellationToken);
            }
        }

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="DbDataReader">data reader</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public Task<DbDataReader> ExecuteReaderAsync(SqlQueryCommand command)
        {
            return ExecuteReaderAsync(command, CancellationToken.None);
        }

        /// <summary>
        ///     Executes the specified command asynchronously.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     A <see cref="Task" /> that will return a <see cref="DbDataReader">data reader</see>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="command" /> is <c>null</c>.</exception>
        public Task<DbDataReader> ExecuteReaderAsync(SqlQueryCommand command, CancellationToken cancellationToken)
        {
            if (command == null) 
                throw new ArgumentNullException("command");

            using (var dbCommand = _dbConnection.CreateCommand())
            {
                dbCommand.Connection = _dbConnection;
                dbCommand.CommandTimeout = _commandTimeout;

                dbCommand.CommandType = command.Type;
                dbCommand.CommandText = command.Text;
                dbCommand.Parameters.AddRange(command.Parameters);
                return dbCommand.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken);
            }
        }
    }
}