using System;
using Projac;

namespace Recipes.Descriptors
{
    /// <summary>
    ///     Represent a SQL projection descriptor.
    /// </summary>
    public class SqlProjectionDescriptor
    {
        private readonly string _identifier;
        private readonly SqlProjection _projection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlProjectionDescriptor"/> class.
        /// </summary>
        /// <param name="identifier">The projection identifier.</param>
        /// <param name="projection">The projection.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/>, <paramref name="version"/> or <paramref name="projection"/> is <c>null</c>.</exception>
        public SqlProjectionDescriptor(string identifier, SqlProjection projection)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (projection == null) throw new ArgumentNullException("projection");
            _identifier = identifier;
            _projection = projection;
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
        /// Gets the projection.
        /// </summary>
        /// <value>
        /// The projection.
        /// </value>
        public SqlProjection Projection
        {
            get { return _projection; }
        }

        /// <summary>
        /// Creates a <see cref="SqlProjectionDescriptorBuilder"/> based on this descriptor.
        /// </summary>
        /// <returns>A <see cref="SqlProjectionDescriptorBuilder"/>.</returns>
        public SqlProjectionDescriptorBuilder ToBuilder()
        {
            return new SqlProjectionDescriptorBuilder(this);
        }
    }
}