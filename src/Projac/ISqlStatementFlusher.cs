using System.Collections.Generic;

namespace Projac {
  /// <summary>
  /// Represents the flush to relational database behavior.
  /// </summary>
  public interface ISqlStatementFlusher {
    /// <summary>
    /// Flushes the specified <paramref name="statements"/> to the underlying relational database.
    /// </summary>
    /// <param name="statements">The sql statements to flush.</param>
    void Flush(IEnumerable<SqlStatement> statements);
  }
}