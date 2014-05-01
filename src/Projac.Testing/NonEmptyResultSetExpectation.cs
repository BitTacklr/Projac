using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class NonEmptyResultSetExpectation : IExpectation
    {
        private readonly TSqlQueryStatement _query;

        public NonEmptyResultSetExpectation(TSqlQueryStatement query)
        {
            _query = query;
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

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.IsClosed && reader.Read())
                    {
                        return new NonEmptyResultSetExpectationVerificationResult(this, ExpectationVerificationResultState.Passed);
                    }
                    return new NonEmptyResultSetExpectationVerificationResult(this, ExpectationVerificationResultState.Failed);
                }
            }
        }
    }
}