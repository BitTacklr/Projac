using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    /// Represents the execution of a set of <see cref="SqlNonQueryStatement">statements</see> against a data source using an open connection.
    /// </summary>
    public class ConnectedSqlNonQueryStatementExecutor : ISqlNonQueryStatementExecutor
    {
        private readonly DbConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlNonQueryStatementExecutor"/> class.
        /// </summary>
        /// <param name="connection">The connection to execute the statements on.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connection"/> is <c>null</c>.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="connection"/> is not open.</exception>
        public ConnectedSqlNonQueryStatementExecutor(DbConnection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (connection.State != ConnectionState.Open)
                throw new ArgumentException("The connection must be in the 'Open' state. Please make sure you've opened the connection beforehand.", "connection");
            _connection = connection;

        }

        /// <summary>
        /// Executes the specified statements.
        /// </summary>
        /// <param name="statements">The statements.</param>
        /// <returns>The number of <see cref="SqlNonQueryStatement">statements</see> executed.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="statements"/> are <c>null</c>.</exception>
        public int Execute(IEnumerable<SqlNonQueryStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");

            using (var command = _connection.CreateCommand())
            {
                command.Connection = _connection;
                command.CommandType = CommandType.Text;
                var count = 0;
                foreach (var statement in statements)
                {
                    command.CommandText = statement.Text;
                    command.Parameters.Clear();
                    command.Parameters.AddRange(statement.Parameters);
                    command.ExecuteNonQuery();
                    count++;
                }
                return count;
            }
        }
    }
}