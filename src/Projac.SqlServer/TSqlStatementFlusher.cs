using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Projac.SqlServer {
  public class TSqlStatementFlusher : ISqlStatementFlusher {
    private readonly SqlConnectionStringBuilder _builder;

    /// <summary>
    /// Initializes a new instance of the <see cref="TSqlStatementFlusher"/> class.
    /// </summary>
    /// <param name="builder">The connection string builder.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="builder"/> is <c>null</c>.</exception>
    public TSqlStatementFlusher(SqlConnectionStringBuilder builder) {
      if (builder == null) throw new ArgumentNullException("builder");
      _builder = builder;
    }

    public void Flush(IEnumerable<SqlStatement> statements) {
      if (statements == null) throw new ArgumentNullException("statements");
      using (var enumerator = statements.GetEnumerator()) {
        var moved = enumerator.MoveNext();
        if (!moved) return;
        using (var connection = new SqlConnection(_builder.ConnectionString)) {
          connection.Open();
          using (var transaction = connection.BeginTransaction()) {
            using (var command = new SqlCommand()) {
              command.Connection = connection;
              command.Transaction = transaction;
              command.CommandType = CommandType.Text;
              while (moved) {
                var statement = enumerator.Current;
                command.CommandText = statement.Text;
                command.Parameters.Clear();
                foreach (var parameter in statement.Parameters) {
                  command.Parameters.AddWithValue("@" + parameter.Item1, parameter.Item2);
                }
                command.ExecuteNonQuery();
                moved = enumerator.MoveNext();
              }
            }
            transaction.Commit();
          }
          connection.Close();
        }
      }
    }
  }
}
