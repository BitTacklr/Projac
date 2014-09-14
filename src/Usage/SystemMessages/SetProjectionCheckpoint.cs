using System;

namespace Usage.SystemMessages
{
    public class SetProjectionCheckpoint
    {
        public readonly string Identifier;
        public readonly long Checkpoint;

        public SetProjectionCheckpoint(string identifier, long checkpoint)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            Identifier = identifier;
            Checkpoint = checkpoint;
        }
    }
}