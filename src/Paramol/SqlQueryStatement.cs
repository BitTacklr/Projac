using System;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represent a SQL query statement.
    /// </summary>
    public class SqlQueryStatement
    {
        private readonly DbParameter[] _parameters;
        private readonly string _text;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlQueryStatement" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown when <paramref name="text" /> or <paramref name="parameters" />
        ///     is <c>null</c>.
        /// </exception>
        public SqlQueryStatement(string text, DbParameter[] parameters)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            //TODO: Remove this because it doesn't make sense when we're dealing with DbParameters
            if (parameters.Length > Limits.MaxParameterCount)
                throw new ArgumentException(
                    string.Format("The parameter count is limited to {0}.", Limits.MaxParameterCount),
                    "parameters");
            _text = text;
            _parameters = parameters;
        }

        /// <summary>
        ///     Gets the text.
        /// </summary>
        /// <value>
        ///     The text.
        /// </value>
        public string Text
        {
            get { return _text; }
        }

        /// <summary>
        ///     Gets the parameters.
        /// </summary>
        /// <value>
        ///     The parameters.
        /// </value>
        public DbParameter[] Parameters
        {
            get { return _parameters; }
        }
    }
}