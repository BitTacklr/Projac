using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    /// Represents the execution of a set of <see cref="SqlNonQueryStatement">statements</see> against a data source.
    /// </summary>
    public class SqlNonQueryStatementExecutor : ISqlNonQueryStatementExecutor
    {
        private readonly ConnectionStringSettings _settings;
        private readonly DbProviderFactory _dbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlNonQueryStatementExecutor"/> class.
        /// </summary>
        /// <param name="settings">The connection string settings.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="settings"/> is <c>null</c>.</exception>
        public SqlNonQueryStatementExecutor(ConnectionStringSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            _settings = settings;
            _dbProviderFactory = DbProviderFactories.GetFactory(settings.ProviderName);
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

            using (var connection = _dbProviderFactory.CreateConnection())
            {
                connection.ConnectionString = _settings.ConnectionString;
                connection.Open();
                try
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Connection = connection;
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
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
