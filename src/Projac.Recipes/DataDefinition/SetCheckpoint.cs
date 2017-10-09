namespace Recipes.DataDefinition
{
    /// <summary>
    /// Instructs a projection to record its checkpoint.
    /// </summary>
    public class SetCheckpoint
    {
        /// <summary>
        /// The checkpoint the projection is at.
        /// </summary>
        public readonly long Checkpoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetCheckpoint"/> class.
        /// </summary>
        /// <param name="checkpoint">The checkpoint the projection is at.</param>
        public SetCheckpoint(long checkpoint)
        {
            Checkpoint = checkpoint;
        }
    }
}