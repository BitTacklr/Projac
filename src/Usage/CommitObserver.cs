using System;
using System.Globalization;
using System.Linq;
using System.Reactive;
using NEventStore;
using Projac;
using Usage.SystemMessages;

namespace Usage
{
    public class CommitObserver : ObserverBase<ICommit>
    {
        private readonly string _identifier;
        private readonly long _checkpoint;
        private readonly SqlProjector _projector;

        public CommitObserver(string identifier, long checkpoint, SqlProjector projector)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (projector == null) throw new ArgumentNullException("projector");
            _identifier = identifier;
            _checkpoint = checkpoint;
            _projector = projector;
        }

        protected override void OnNextCore(ICommit value)
        {
            var commitCheckpoint = Int64.Parse(value.CheckpointToken, CultureInfo.InvariantCulture);
            Console.WriteLine("Projection {0} is handling commit at checkpoint {1}.", _identifier, commitCheckpoint);
            if (commitCheckpoint <= _checkpoint) return;
            _projector.Project(
                value.
                    Events.
                    Select(_ => _.Body).
                    Concat(new object[]
                    {
                        new SetProjectionCheckpoint(_identifier, commitCheckpoint)
                    })
                );
        }

        protected override void OnErrorCore(Exception error)
        {
        }

        protected override void OnCompletedCore()
        {
        }
    }
}