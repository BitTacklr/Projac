using System;
using System.Globalization;
using System.Linq;
using NEventStore;
using Projac;
using Usage.SystemMessages;

namespace Usage
{
    public class CommitObserver : IObserver<ICommit>
    {
        private readonly string _identifier;
        private readonly SqlProjector _projector;

        public CommitObserver(string identifier, SqlProjector projector)
        {
            if (identifier == null) throw new ArgumentNullException("identifier");
            if (projector == null) throw new ArgumentNullException("projector");
            _identifier = identifier;
            _projector = projector;
        }

        public void OnNext(ICommit value)
        {
            _projector.Project(
                value.
                    Events.
                    Select(_ => _.Body).
                    Concat(new object[]
                    {
                        new SetProjectionCheckpoint(_identifier, Int64.Parse(value.CheckpointToken, CultureInfo.InvariantCulture))
                    })
                );
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}