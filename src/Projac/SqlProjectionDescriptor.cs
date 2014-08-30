using System;
using System.Collections.Generic;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represent a projection descriptor.
    /// </summary>
    public class SqlProjectionDescriptor
    {
        private readonly Uri _identifier;
        private readonly SqlProjection _projection;
        private readonly SqlNonQueryStatement[] _dataDefinitionStatements;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptor"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <param name="dataDefinitionStatements">The data definition statements.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/>, <paramref name="dataDefinitionStatements"/> or <paramref name="projection"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptor(Uri identifier, SqlNonQueryStatement[] dataDefinitionStatements, SqlProjection projection)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (dataDefinitionStatements == null) throw new ArgumentNullException("dataDefinitionStatements");
            if (projection == null) throw new ArgumentNullException("projection");
            _identifier = identifier;
            _projection = projection;
            _dataDefinitionStatements = dataDefinitionStatements;
        }

        /// <summary>
        /// Gets the projection identifier.
        /// </summary>
        /// <value>
        /// The projection identifier.
        /// </value>
        public Uri Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Gets the data definition statements.
        /// </summary>
        /// <value>
        /// The data definition statements.
        /// </value>
        public IReadOnlyCollection<SqlNonQueryStatement> DataDefinitionStatements
        {
            get { return _dataDefinitionStatements; }
        }

        /// <summary>
        /// Gets the projection.
        /// </summary>
        /// <value>
        /// The projection.
        /// </value>
        public SqlProjection Projection
        {
            get { return _projection; }
        }
    }
}