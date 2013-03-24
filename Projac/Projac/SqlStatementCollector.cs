using System;
using System.Collections.Generic;

namespace Projac {
  public class SqlStatementCollector : IObserver<SqlStatement> {
    private readonly List<SqlStatement> _statements;

    public SqlStatementCollector() {
      _statements = new List<SqlStatement>();
    }

    public void OnNext(SqlStatement statement) {
      _statements.Add(statement);
    }

    public void OnError(Exception error) {}

    public void OnCompleted() {}

    public SqlStatement[] Statements {
      get { return _statements.ToArray(); }
    }
  }
}