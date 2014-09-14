using System;

namespace Usage.SystemMessages
{
    public class RebuildProjection
    {
        public readonly string Identifier;
        public readonly string OldVersion;
        public readonly string NewVersion;

        public RebuildProjection(string identifier, string oldVersion, string newVersion)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (oldVersion == null) throw new ArgumentNullException("oldVersion");
            if (newVersion == null) throw new ArgumentNullException("newVersion");
            Identifier = identifier;
            OldVersion = oldVersion;
            NewVersion = newVersion;
        }
    }
}