using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac {
  /// <summary>
  /// Syntatic sugar factory that creates <see cref="SqlStatement"/>s, to be used in projection handlers.
  /// </summary>
  public static class Sql {
    /// <summary>
    /// Creates a new <see cref="SqlStatement"/> using the specified text and parameters.
    /// </summary>
    /// <param name="text">The text of the sql statement.</param>
    /// <param name="parameters">The parameters of the sql statement, or <c>null</c> if none.</param>
    /// <returns>A <see cref="SqlStatement"/> constructed using the specified text and parameters.</returns>
    public static SqlStatement Statement(string text, object parameters = null) {
      return new SqlStatement(text, ExtractProperties(parameters));
    }

    private static IEnumerable<Tuple<string, object>> ExtractProperties(object parameters) {
      return parameters == null
               ? new Tuple<string, object>[0]
               : parameters.
                   GetType().
                   GetProperties().
                   OrderBy(property => property.Name).
                   Select(property => new Tuple<string, object>(property.Name, property.GetValue(parameters)));
    }
  }
}