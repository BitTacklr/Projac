using System;
using System.Collections.Generic;

namespace Projac.Tests.Builders {
  public class SqlStatementCollectorBuilder {
    private IEnumerable<SqlStatement> _statements;

    public SqlStatementCollectorBuilder() {
      _statements = new SqlStatement[0];
    }

    public SqlStatementCollectorBuilder WithStatements(IEnumerable<SqlStatement> value) {
      if (value == null) throw new ArgumentNullException("value");
      _statements = value;
      return this;
    }

    public SqlStatementCollector Build() {
      var collector = new SqlStatementCollector();
      foreach (var statement in _statements) {
        collector.OnNext(statement);
      }
      return collector;
    }
  }
}
