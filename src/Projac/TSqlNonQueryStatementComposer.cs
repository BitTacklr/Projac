using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac
{
    /// <summary>
    /// Represents a composition of T-SQL non query statements.
    /// </summary>
    public class TSqlNonQueryStatementComposer
    {
        private readonly TSqlNonQueryStatement[] _statements;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlNonQueryStatementComposer"/> class.
        /// </summary>
        /// <param name="statements">The statements composed so far.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlNonQueryStatementComposer(TSqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            _statements = statements;
        }

        /// <summary>
        /// Composes this instance with the specified <paramref name="statements"/>.
        /// </summary>
        /// <param name="statements">The <see cref="TSqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="TSqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlNonQueryStatementComposer Compose(params TSqlNonQueryStatement[] statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlNonQueryStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        /// Composes this instance with the specified <paramref name="statements"/>.
        /// </summary>
        /// <param name="statements">The <see cref="TSqlNonQueryStatement">statements</see> to compose with.</param>
        /// <returns>A new composition of <see cref="TSqlNonQueryStatement">statements</see>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="statements"/> are <c>null</c>.</exception>
        public TSqlNonQueryStatementComposer Compose(IEnumerable<TSqlNonQueryStatement> statements)
        {
            if (statements == null) throw new ArgumentNullException("statements");
            return new TSqlNonQueryStatementComposer(_statements.Concat(statements).ToArray());
        }

        /// <summary>
        /// Implicitly converts a composition of <see cref="TSqlNonQueryStatement">statements</see> to an array of <see cref="TSqlNonQueryStatement">statements</see>.
        /// </summary>
        /// <param name="instance">The instance to convert.</param>
        /// <returns>An array of <see cref="TSqlNonQueryStatement">statements</see>.</returns>
        public static implicit operator TSqlNonQueryStatement[](TSqlNonQueryStatementComposer instance)
        {
            return instance._statements;
        }
    }
}