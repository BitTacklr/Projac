using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Projac.Testing
{
    /// <summary>
    /// Represents a projection test specification runner.
    /// </summary>
    public class TSqlProjectionTestSpecificationRunner
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlProjectionTestSpecificationRunner"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="connectionString"/> is <c>null</c>.</exception>
        public TSqlProjectionTestSpecificationRunner(string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            _connectionString = connectionString;
        }

        /// <summary>
        /// Runs the specified projection specification and verifies the expectations.
        /// </summary>
        /// <param name="specification">The projection specification to run.</param>
        /// <returns>The result of the run.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="specification"/> is <c>null</c>.</exception>
        public TSqlProjectionTestResult Run(TSqlProjectionTestSpecification specification)
        {
            if (specification == null) throw new ArgumentNullException("specification");
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                try
                {
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            using (var command = new SqlCommand())
                            {
                                command.Connection = connection;
                                command.Transaction = transaction;
                                command.CommandType = CommandType.Text;
                                //Givens
                                foreach (var givenStatement in 
                                    from given in specification.Givens 
                                    from handler in specification.Projection.Handlers
                                    where handler.Event == given.GetType()
                                    from statement in handler.Handler(given)
                                    select statement)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.AddRange(givenStatement.Parameters);
                                    command.CommandText = givenStatement.Text;
                                    command.ExecuteNonQuery();
                                }
                                //When
                                foreach (var whenStatement in
                                    from handler in specification.Projection.Handlers
                                    where handler.Event == specification.When.GetType()
                                    from statement in handler.Handler(specification.When)
                                    select statement)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.AddRange(whenStatement.Parameters);
                                    command.CommandText = whenStatement.Text;
                                    command.ExecuteNonQuery();
                                }
                                //Then
                                foreach (var verification in specification.Expectations)
                                {
                                    if (!verification.IsSatisfied(transaction))
                                    {
                                        return new TSqlProjectionTestResult(); //Fail
                                    }
                                }
                                return new TSqlProjectionTestResult(); //Pass
                            }
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
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