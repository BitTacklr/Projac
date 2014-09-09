using System;
using Paramol;

namespace Projac
{
    /// <summary>
    ///     Represent a SQL projection descriptor builder.
    /// </summary>
    public class SqlProjectionDescriptorBuilder
    {
        private readonly string _identifier;
        private SqlProjection _projection;
        private SqlNonQueryCommand[] _dataDefinitionCommands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptorBuilder"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptorBuilder(string identifier)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            _identifier = identifier;
            _projection = SqlProjection.Empty;
            _dataDefinitionCommands = new SqlNonQueryCommand[0];
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
        /// Gets or sets the data definition commands.
        /// </summary>
        /// <value>
        /// The data definition commands.
        /// </value>
        public SqlNonQueryCommand[] DataDefinitionCommands
        {
            get { return _dataDefinitionCommands; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _dataDefinitionCommands = value;
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
            return new SqlProjectionDescriptor(Identifier, DataDefinitionCommands, Projection);
        }
    }
}