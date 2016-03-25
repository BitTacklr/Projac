using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramol.SqlClient
{
    public partial class SqlClientSyntax
    {
        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer Compose(IEnumerable<SqlNonQueryCommand> commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(commands.ToArray());
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeIf(bool condition,
            IEnumerable<SqlNonQueryCommand> commands)
        {
            return Compose(condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> unless the condition is
        ///     satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeUnless(bool condition,
            IEnumerable<SqlNonQueryCommand> commands)
        {
            return Compose(!condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" />.
        /// </summary>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer Compose(params SqlNonQueryCommand[] commands)
        {
            if (commands == null) throw new ArgumentNullException("commands");
            return new SqlNonQueryCommandComposer(commands);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeIf(bool condition, params SqlNonQueryCommand[] commands)
        {
            return Compose(condition ? commands : new SqlNonQueryCommand[0]);
        }

        /// <summary>
        ///     Starts a composition of commands with the specified <paramref name="commands" /> unless the condition is
        ///     satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="commands">The <see cref="SqlNonQueryCommand">commands</see> to start the composition with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryCommand">commands</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="commands" /> are <c>null</c>.</exception>
        public SqlNonQueryCommandComposer ComposeUnless(bool condition,
            params SqlNonQueryCommand[] commands)
        {
            return Compose(!condition ? commands : new SqlNonQueryCommand[0]);
        }
    }
}
