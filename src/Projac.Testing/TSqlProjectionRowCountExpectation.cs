using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class TSqlProjectionRowCountExpectation : ITSqlProjectionExpectation
    {
        private readonly TSqlQueryStatement _query;
        private readonly int _rowCount;

        public TSqlProjectionRowCountExpectation(TSqlQueryStatement query, int rowCount)
        {
            _query = query;
            _rowCount = rowCount;
        }

        public bool Verify(SqlTransaction transaction)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(_query.Parameters);
                command.CommandText = _query.Text;

                var result = (int)command.ExecuteScalar();
                return result.Equals(_rowCount);
            }
        }
    }
}