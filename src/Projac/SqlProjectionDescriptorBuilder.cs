using System;

namespace Projac
{
    /// <summary>
    ///     Represent a SQL projection descriptor builder.
    /// </summary>
    public class SqlProjectionDescriptorBuilder
    {
        private readonly string _identifier;
        private readonly string _version;
        private SqlProjection _schemaProjection;
        private SqlProjection _projection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptorBuilder"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <param name="version">The projection version.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/> or <paramref name="version"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptorBuilder(string identifier, string version)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (version == null) throw new ArgumentNullException("version");
            _identifier = identifier;
            _version = version;
            _schemaProjection = SqlProjection.Empty;
            _projection = SqlProjection.Empty;
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
        /// Gets the projection version.
        /// </summary>
        /// <value>
        /// The projection version.
        /// </value>
        public string Version
        {
            get { return _version; }
        }

        /// <summary>
        /// Gets or sets the schema projection.
        /// </summary>
        /// <value>
        /// The schema projection.
        /// </value>
        public SqlProjection SchemaProjection
        {
            get { return _schemaProjection; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _schemaProjection = value;
            }
        }

        /// <summary>
        /// Gets or sets the projection.
        /// </summary>
        /// <value>
        /// The projection.
        /// </value>
        public SqlProjection Projection
        {
            get { return _projection; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _projection = value;
            }
        }

        /// <summary>
        /// Builds a <see cref="SqlProjectionDescriptor"/>.
        /// </summary>
        /// <returns>A <see cref="SqlProjectionDescriptor"/>.</returns>
        public SqlProjectionDescriptor Build()
        {
            return new SqlProjectionDescriptor(Identifier, Version, SchemaProjection, Projection);
        }
    }
}