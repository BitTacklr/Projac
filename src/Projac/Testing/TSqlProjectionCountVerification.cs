using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class TSqlProjectionCountVerification : ITSqlProjectionVerification
    {
        private readonly TSqlQueryStatement _query;
        private readonly int _count;

        public TSqlProjectionCountVerification(TSqlQueryStatement query, int count)
        {
            _query = query;
            _count = count;
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
                return result == _count;
            }
        }
    }
}