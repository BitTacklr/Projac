using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    /// Represents the execution of a set of <see cref="SqlNonQueryStatement">statements</see> against a data source using a begun transaction.
    /// </summary>
    public class ConnectedTransactionalSqlNonQueryStatementExecutor : ISqlNonQueryStatementExecutor
    {
        private readonly DbTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionalSqlNonQueryStatementExecutor"/> class.
        /// </summary>
        /// <param name="transaction">The transaction to execute the statements on.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="transaction"/> is <c>null</c>.</exception>
        public ConnectedTransactionalSqlNonQueryStatementExecutor(DbTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            _transaction = transaction;
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

            var count = 0;
            using (var command = _transaction.Connection.CreateCommand())
            {
                command.Connection = _transaction.Connection;
                command.Transaction = _transaction;
                command.CommandType = CommandType.Text;
                foreach (var statement in statements)
                {
                    command.CommandText = statement.Text;
                    command.Parameters.Clear();
                    command.Parameters.AddRange(statement.Parameters);
                    command.ExecuteNonQuery();
                    count++;
                }
            }
            return count;
        }
    }
}