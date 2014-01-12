using System;
using System.Data.SqlClient;

namespace Projac
{
    /// <summary>
    /// Represent a T-SQL non query statement.
    /// </summary>
    public class TSqlNonQueryStatement
    {
        private readonly string _text;
        private readonly SqlParameter[] _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlNonQueryStatement"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="text"/> or <paramref name="parameters"/> is <c>null</c>.</exception>
        public TSqlNonQueryStatement(string text, SqlParameter[] parameters)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (parameters == null) throw new ArgumentNullException("parameters");
            _text = text;
            _parameters = parameters;
        }

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text
        {
            get
            {
                return _text;
            }
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public SqlParameter[] Parameters
        {
            get
            {
                return _parameters;
            }
        }
    }
}