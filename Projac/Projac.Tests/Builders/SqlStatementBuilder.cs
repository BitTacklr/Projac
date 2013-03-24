using System;
using System.Collections.Generic;

namespace Projac.Tests.Builders {
  public class SqlStatementBuilder {
    private string _text;
    private IEnumerable<Tuple<string, object>> _parameters;

    public SqlStatementBuilder() {
      _text = string.Empty;
      _parameters = new Tuple<string, object>[0];
    }

    public SqlStatementBuilder WithText(string value) {
      _text = value;
      return this;
    }

    public SqlStatementBuilder WithParameters(IEnumerable<Tuple<string, object>> value) {
      _parameters = value;
      return this;
    }

    public SqlStatement Build() {
      return new SqlStatement(_text, _parameters);  
    }
  }
}