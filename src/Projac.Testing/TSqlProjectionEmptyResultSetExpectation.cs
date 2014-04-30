using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class TSqlProjectionEmptyResultSetExpectation : ITSqlProjectionExpectation
    {
        private readonly TSqlQueryStatement _query;

        public TSqlProjectionEmptyResultSetExpectation(TSqlQueryStatement query)
        {
            _query = query;
        }

        public bool IsSatisfied(SqlTransaction transaction)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(_query.Parameters);
                command.CommandText = _query.Text;

                using (var reader = command.ExecuteReader())
                {
                    return reader.IsClosed || !reader.Read();
                }
            }
        }
    }
}