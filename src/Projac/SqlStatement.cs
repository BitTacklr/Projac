using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac {
  public class SqlStatement {
    private readonly string _text;
    private readonly Tuple<string, object>[] _parameters;

    public SqlStatement(string text, IEnumerable<Tuple<string, object>> parameters) {
      if (text == null) throw new ArgumentNullException("text");
      if (parameters == null) throw new ArgumentNullException("parameters");
      _text = text;
      _parameters = parameters.ToArray();
    }

    public string Text {
      get { return _text; }
    }

    public IEnumerable<Tuple<string, object>> Parameters {
      get { return _parameters; }
    }

    public override bool Equals(object obj) {
      if (ReferenceEquals(this, obj)) return true;
      if (ReferenceEquals(obj, null)) return false;
      if (GetType() != obj.GetType()) return false;
      var other = (SqlStatement) obj;
      return Text.Equals(other.Text) && Parameters.SequenceEqual(other.Parameters);
    }

    public override int GetHashCode() {
      return Parameters.
        Aggregate(
          Text.GetHashCode(),
          (current, parameter) => current ^ parameter.GetHashCode());
    }
  }
}