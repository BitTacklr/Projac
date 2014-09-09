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
        private readonly SqlNonQueryCommand[] _dataDefinitionCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptor"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <param name="dataDefinitionCommands">The data definition commands.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/>, <paramref name="dataDefinitionCommands"/> or <paramref name="projection"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptor(string identifier, SqlNonQueryCommand[] dataDefinitionCommands, SqlProjection projection)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (dataDefinitionCommands == null) throw new ArgumentNullException("dataDefinitionStatements");
            if (projection == null) throw new ArgumentNullException("projection");
            _identifier = identifier;
            _projection = projection;
            _dataDefinitionCommands = dataDefinitionCommands;
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
        /// Gets the data definition commands.
        /// </summary>
        /// <value>
        /// The data definition commands.
        /// </value>
        public SqlNonQueryCommand[] DataDefinitionCommands
        {
            get { return _dataDefinitionCommands; }
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