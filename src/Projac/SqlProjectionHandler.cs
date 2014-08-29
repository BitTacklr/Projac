using System;
using System.Collections.Generic;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represents a handler of a particular type of event.
    /// </summary>
    public class SqlProjectionHandler
    {
        private readonly Type _event;
        private readonly Func<object, IEnumerable<SqlNonQueryStatement>> _handler;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlProjectionHandler" /> class.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="handler">The handler.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     Throw when <paramref name="event" /> or <paramref name="handler" /> is
        ///     <c>null</c>.
        /// </exception>
        public SqlProjectionHandler(Type @event, Func<object, IEnumerable<SqlNonQueryStatement>> handler)
        {
            if (@event == null) throw new ArgumentNullException("event");
            if (handler == null) throw new ArgumentNullException("handler");
            _event = @event;
            _handler = handler;
        }

        /// <summary>
        ///     The type of event to handle.
        /// </summary>
        public Type Event
        {
            get { return _event; }
        }

        /// <summary>
        ///     The function that handles the event.
        /// </summary>
        public Func<object, IEnumerable<SqlNonQueryStatement>> Handler
        {
            get { return _handler; }
        }
    }
}