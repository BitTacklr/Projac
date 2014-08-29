using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramol
{
    /// <summary>
    ///     Represents a composition of T-SQL non query statements.
    /// </summary>
    public class SqlNonQueryStatementComposer
    {
        private readonly SqlNonQueryStatement[] _statements;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlNonQueryStatementComposer" /> class.
        /// </summary>
        /// <param name="statements">The statements composed so far.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer(SqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            _statements = statements;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer Compose(params SqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer ComposeIf(bool condition, params SqlNonQueryStatement[] statements)
        {
            return condition
                ? new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer ComposeUnless(bool condition, params SqlNonQueryStatement[] statements)
        {
            return !condition
                ? new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" />.
        /// </summary>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer Compose(IEnumerable<SqlNonQueryStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" /> if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer ComposeIf(bool condition, IEnumerable<SqlNonQueryStatement> statements)
        {
            return condition
                ? new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray())
                : this;
        }

        /// <summary>
        ///     Composes this instance with the specified <paramref name="statements" /> unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy.</param>
        /// <param name="statements">The <see cref="SqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements" /> are <c>null</c>.</exception>
        public SqlNonQueryStatementComposer ComposeUnless(bool condition, IEnumerable<SqlNonQueryStatement> statements)
        {
            return !condition
                ? new SqlNonQueryStatementComposer(_statements.Concat(statements).ToArray())
                : this;
        }

        /// <summary>
        ///     Implicitly converts a composition of <see cref="SqlNonQueryStatement">statements</see> to an array of
        ///     <see cref="SqlNonQueryStatement">statements</see>.
        /// </summary>
        /// <param name="instance">The instance to convert.</param>
        /// <returns>An array of <see cref="SqlNonQueryStatement">statements</see>.</returns>
        public static implicit operator SqlNonQueryStatement[](SqlNonQueryStatementComposer instance)
        {
            return instance._statements;
        }
    }
}