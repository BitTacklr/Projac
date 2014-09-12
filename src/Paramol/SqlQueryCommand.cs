using System;
using System.Data;
using System.Data.Common;

namespace Paramol
{
    /// <summary>
    ///     Represent a SQL query command.
    /// </summary>
    public class SqlQueryCommand
    {
        private readonly DbParameter[] _parameters;
        private readonly string _text;
        private readonly CommandType _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlQueryCommand" /> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="type">The type of command.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Thrown when <paramref name="text" /> or <paramref name="parameters" />
        ///     is <c>null</c>.
        /// </exception>
        public SqlQueryCommand(string text, DbParameter[] parameters, CommandType type)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            if (!Enum.IsDefined(typeof(CommandType), type))
                throw new ArgumentException(string.Format("The command type value {0} is not supported.", type), "type");
            _text = text;
            _parameters = parameters;
            _type = type;
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

        /// <summary>
        ///     Gets the type of command.
        /// </summary>
        /// <value>
        ///     The type of command.
        /// </value>
        public CommandType Type
        {
            get { return _type; }
        }
    }
}