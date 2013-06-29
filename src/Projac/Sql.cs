using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac {
  public static class Sql {
    public static SqlStatement Statement(string text, object parameters = null) {
      return parameters == null ? 
        new SqlStatement(text, new Tuple<string, object>[0]) : 
        new SqlStatement(text, ExtractProperties(parameters));
    }

    private static IEnumerable<Tuple<string, object>> ExtractProperties(object parameters) {
      return parameters == null
               ? new Tuple<string, object>[0]
               : parameters.
                   GetType().
                   GetProperties().
                   OrderBy(property => property.Name).
                   Select(property => new Tuple<string, object>(property.Name, property.GetValue(parameters))).
                   ToArray();
    }
  }
}