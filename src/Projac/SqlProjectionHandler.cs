using System;
using System.Collections.Generic;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represents a handler of a particular type of message.
    /// </summary>
    public class SqlProjectionHandler
    {
        private readonly Type _message;
        private readonly Func<object, IEnumerable<SqlNonQueryCommand>> _handler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjectionHandler" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="message" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public SqlProjectionHandler(Type message, Func<object, IEnumerable<SqlNonQueryCommand>> handler)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (handler == null) throw new ArgumentNullException("handler");
            _message = message;
            _handler = handler;
        }

        /// <summary>
        ///     The type of message to handle.
        /// </summary>
        public Type Message
        {
            get { return _message; }
        }

        /// <summary>
        ///     The function that handles the message.
        /// </summary>
        public Func<object, IEnumerable<SqlNonQueryCommand>> Handler
        {
            get { return _handler; }
        }
    }
}