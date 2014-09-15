using System;

namespace Projac.Messages
{
    /// <summary>
    /// Rebuild a projection.
    /// </summary>
    public class RebuildProjection
    {
        /// <summary>
        /// The projection identifier.
        /// </summary>
        public readonly string Identifier;
        /// <summary>
        /// The version of the projection to be dropped.
        /// </summary>
        public readonly string DropVersion;
        /// <summary>
        /// The version of the projection to be created.
        /// </summary>
        public readonly string CreateVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="RebuildProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="dropVersion">The drop version.</param>
        /// <param name="createVersion">The create version.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/>, <paramref name="dropVersion"/> or <paramref name="createVersion"/> is <c>null</c>.</exception>
        public RebuildProjection(string identifier, string dropVersion, string createVersion)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (dropVersion == null) throw new ArgumentNullException("dropVersion");
            if (createVersion == null) throw new ArgumentNullException("createVersion");
            Identifier = identifier;
            DropVersion = dropVersion;
            CreateVersion = createVersion;
        }
    }
}
