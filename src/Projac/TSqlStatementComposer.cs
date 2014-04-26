using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac
{
    /// <summary>
    /// Represents a composition of T-SQL statements.
    /// </summary>
    public class TSqlStatementComposer
    {
        private readonly ITSqlStatement[] _statements;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlStatementComposer"/> class.
        /// </summary>
        /// <param name="statements">The statements composed so far.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlStatementComposer(ITSqlStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            _statements = statements;
        }

        /// <summary>
        /// Composes this instance with the specified <paramref name="statements"/>.
        /// </summary>
        /// <param name="statements">The <see cref="ITSqlStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="ITSqlStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlStatementComposer Compose(params ITSqlStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        /// Composes this instance with the specified <paramref name="statements"/>.
        /// </summary>
        /// <param name="statements">The <see cref="ITSqlStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="ITSqlStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlStatementComposer Compose(IEnumerable<ITSqlStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        /// Implicitly converts a composition of <see cref="ITSqlStatement">statements</see> to an array of <see cref="ITSqlStatement">statements</see>.
        /// </summary>
        /// <param name="instance">The instance to convert.</param>
        /// <returns>An array of <see cref="ITSqlStatement">statements</see>.</returns>
        public static implicit operator ITSqlStatement[](TSqlStatementComposer instance)
        {
            return instance._statements;
        }
    }
}