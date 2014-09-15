using System;

namespace Projac.Messages
{
    /// <summary>
    /// Replays a projection.
    /// </summary>
    public class ReplayProjection
    {
        /// <summary>
        /// The projection identifier.
        /// </summary>
        public readonly string Identifier;
        /// <summary>
        /// The version of the projection to be replayed.
        /// </summary>
        public readonly string ReplayVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplayProjection"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="replayVersion">The replay version.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="identifier"/> or <paramref name="replayVersion"/> is <c>null</c>.</exception>
        public ReplayProjection(string identifier, string replayVersion)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (replayVersion == null) throw new ArgumentNullException("replayVersion");
            Identifier = identifier;
            ReplayVersion = replayVersion;
        }
    }
}