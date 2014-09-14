using System;

namespace Usage.SystemMessages
{
    public class BuildProjection
    {
        public readonly string Identifier;
        public readonly string Version;

        public BuildProjection(string identifier, string version)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (version == null) throw new ArgumentNullException("version");
            Identifier = identifier;
            Version = version;
        }
    }
}