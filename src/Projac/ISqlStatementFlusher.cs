using System.Collections.Generic;

namespace Projac {
  public interface ISqlStatementFlusher {
    void Flush(IEnumerable<SqlStatement> statements);
  }
}