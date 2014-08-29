using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Paramol.Tests.Usage
{
    public class TSqlNonQueryStatementFlusher
    {
        private readonly string _connectionString;
        private readonly int _commandTimeout;

        public TSqlNonQueryStatementFlusher(string connectionString, int commandTimeout)
        {
            if (connectionString == null) 
                throw new ArgumentNullException("connectionString");
            if (commandTimeout < 0) 
                throw new ArgumentOutOfRangeException("commandTimeout", commandTimeout, "The command timeout must be greater than or equal to 0.");
            _connectionString = connectionString;
            _commandTimeout = commandTimeout;
        }

        public void Flush(IEnumerable<SqlNonQueryStatement> statements)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandTimeout = _commandTimeout;
                        command.Connection = connection;
                        command.Transaction = transaction;
                        foreach (var statement in statements)
                        {
                            command.CommandText = statement.Text;
                            command.Parameters.Clear();
                            command.Parameters.AddRange(statement.Parameters);

                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                connection.Close();
            }
        }
    }
}
