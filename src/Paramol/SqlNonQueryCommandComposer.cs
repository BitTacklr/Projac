using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramol
{
    /// <summary>
    ///     Represents a composition of SQL non query commands.
    /// </summary>
    public class SqlNonQueryCommandComposer
    {
        private readonly SqlNonQueryCommand[] _commands;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryCommandComposer" /> class.
        /// </summary>
        /// <param name="commands">The commands composed so far.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer(SqlNonQueryCommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            _commands = commands;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer Compose(params SqlNonQueryCommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray());
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeIf(bool condition, params SqlNonQueryCommand[] commands)
        {
            return condition
                ? new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeUnless(bool condition, params SqlNonQueryCommand[] commands)
        {
            return !condition
                ? new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer Compose(IEnumerable<SqlNonQueryCommand> commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray());
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeIf(bool condition, IEnumerable<SqlNonQueryCommand> commands)
        {
            return condition
                ? new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="commands" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeUnless(bool condition, IEnumerable<SqlNonQueryCommand> commands)
        {
            return !condition
                ? new SqlNonQueryCommandComposer(_commands.Concat(commands).ToArray())
                : this;
        }

        /// <summary>
        ///     Implicitly converts a composition of <see cref="SqlNonQueryCommand">commands</see> to an array of
        ///     <see cref="SqlNonQueryCommand">commands</see>.
        /// </summary>
        /// <param name="instance">The instance to convert.</param>
        /// <returns>An array of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        public static implicit operator SqlNonQueryCommand[](SqlNonQueryCommandComposer instance)
        {
            return instance._commands;
        }
    }
}