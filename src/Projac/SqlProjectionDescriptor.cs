using System;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represent a SQL projection descriptor.
    /// </summary>
    public class SqlProjectionDescriptor
    {
        private readonly string _identifier;
        private readonly SqlProjection _projection;
        private readonly SqlNonQueryStatement[] _dataDefinitionStatements;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptor"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <param name="dataDefinitionStatements">The data definition statements.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/>, <paramref name="dataDefinitionStatements"/> or <paramref name="projection"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptor(string identifier, SqlNonQueryStatement[] dataDefinitionStatements, SqlProjection projection)
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
        public string Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Gets the data definition statements.
        /// </summary>
        /// <value>
        /// The data definition statements.
        /// </value>
        public SqlNonQueryStatement[] DataDefinitionStatements
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