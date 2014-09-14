using System;

namespace Usage.SystemMessages
{
    public class ResetProjectionCheckpoint
    {
        public readonly string Identifier;
        public readonly long Checkpoint;

        public ResetProjectionCheckpoint(string identifier, long checkpoint)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            Identifier = identifier;
            Checkpoint = checkpoint;
        }
    }
}