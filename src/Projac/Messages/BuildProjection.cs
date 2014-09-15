using System;

namespace Projac.Messages
{
    /// <summary>
    /// Build a projection.
    /// </summary>
    public class BuildProjection
    {
        /// <summary>
        /// The projection identifier.
        /// </summary>
        public readonly string Identifier;
        /// <summary>
        /// The version of the projection to be created.
        /// </summary>
        public readonly string CreateVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="createVersion">The create version.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/> or <paramref name="createVersion"/> is <c>null</c>.</exception>
        public BuildProjection(string identifier, string createVersion)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (createVersion == null) throw new ArgumentNullException("createVersion");
            Identifier = identifier;
            CreateVersion = createVersion;
        }
    }
}