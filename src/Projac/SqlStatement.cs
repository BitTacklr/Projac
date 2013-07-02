using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Projac {
  /// <summary>
  /// Represents a SQL statement to be sent to a relational database.
  /// </summary>
  public class SqlStatement {
    private readonly string _text;
    private readonly Tuple<string, object>[] _parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="SqlStatement"/> class.
    /// </summary>
    /// <param name="text">The text of the statement.</param>
    /// <param name="parameters">The parameters of the statement.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="parameters"/> is <c>null</c>.</exception>
    public SqlStatement(string text, IEnumerable<Tuple<string, object>> parameters) {
      if (text == null) throw new ArgumentNullException("text");
      if (parameters == null) throw new ArgumentNullException("parameters");
      _text = text;
      _parameters = parameters.ToArray();
    }

    /// <summary>
    /// Gets the text of the sql statement.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text {
      get { return _text; }
    }

    /// <summary>
    /// Gets the parameters of the sql statement.
    /// </summary>
    /// <value>
    /// The parameters.
    /// </value>
    public IList<Tuple<string, object>> Parameters {
      get { return new ReadOnlyCollection<Tuple<string, object>>(_parameters); }
    }

    //public override bool Equals(object obj) {
    //  if (ReferenceEquals(this, obj)) return true;
    //  if (ReferenceEquals(obj, null)) return false;
    //  if (GetType() != obj.GetType()) return false;
    //  var other = (SqlStatement) obj;
    //  return Text.Equals(other.Text) && Parameters.SequenceEqual(other.Parameters);
    //}

    //public override int GetHashCode() {
    //  return Parameters.
    //    Aggregate(
    //      Text.GetHashCode(),
    //      (current, parameter) => current ^ parameter.GetHashCode());
    //}
  }
}