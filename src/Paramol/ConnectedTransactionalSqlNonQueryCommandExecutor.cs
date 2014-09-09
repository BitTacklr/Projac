using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represents the execution of a set of <see cref="SqlNonQueryCommand">commands</see> against a data source using
    ///     a begun transaction.
    /// </summary>
    public class ConnectedTransactionalSqlNonQueryCommandExecutor : ISqlNonQueryCommandExecutor
    {
        private readonly DbTransaction _dbTransaction;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionalSqlNonQueryCommandExecutor" /> class.
        /// </summary>
        /// <param name="dbTransaction">The transaction to execute the commands on.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="dbTransaction" /> is <c>null</c>.</exception>
        public ConnectedTransactionalSqlNonQueryCommandExecutor(DbTransaction dbTransaction)
        {
            if (dbTransaction == null) throw new ArgumentNullException("dbTransaction");
            _dbTransaction = dbTransaction;
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

            var count = 0;
            using (var dbCommand = _dbTransaction.Connection.CreateCommand())
            {
                dbCommand.Connection = _dbTransaction.Connection;
                dbCommand.Transaction = _dbTransaction;

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
    }
}