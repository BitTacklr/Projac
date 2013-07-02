using System.Data.SqlClient;
using Projac.SqlServer;

namespace Projac.Tests.Builders {
  public class TSqlStatementFlusherBuilder {
    private SqlConnectionStringBuilder _builder;

    public TSqlStatementFlusherBuilder() {
      _builder = new SqlConnectionStringBuilder();
    }

    public TSqlStatementFlusherBuilder WithConnectionStringBuilder(SqlConnectionStringBuilder value) {
      _builder = value;
      return this;
    }

    public TSqlStatementFlusher Build() {
      return new TSqlStatementFlusher(_builder);
    }
  }
}