using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class RowCountExpectation : IExpectation
    {
        private readonly TSqlQueryStatement _query;
        private readonly int _rowCount;

        public RowCountExpectation(TSqlQueryStatement query, int rowCount)
        {
            _query = query;
            _rowCount = rowCount;
        }

        public ExpectationVerificationResult Verify(SqlTransaction transaction)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = transaction.Connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                command.Parameters.AddRange(_query.Parameters);
                command.CommandText = _query.Text;

                var result = (int)command.ExecuteScalar();
                if (result.Equals(_rowCount))
                {
                    return new RowCountExpectationVerificationPassResult(this);
                }
                return new RowCountExpectationVerificationFailResult(this, result);
            }
        }
    }
}