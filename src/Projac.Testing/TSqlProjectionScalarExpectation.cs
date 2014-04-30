using System;
using System.Data;
using System.Data.SqlClient;

namespace Projac.Testing
{
    class TSqlProjectionScalarExpectation<TScalar> : ITSqlProjectionExpectation
        where TScalar : IEquatable<TScalar>
    {
        private readonly TSqlQueryStatement _query;
        private readonly TScalar _scalar;

        public TSqlProjectionScalarExpectation(TSqlQueryStatement query, TScalar scalar)
        {
            _query = query;
            _scalar = scalar;
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

                var result = (TScalar)command.ExecuteScalar();
                return result.Equals(_scalar);
            }
        }
    }
}