using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Projac.Tests
{
    public class CallRecordingConnection
    {
        private readonly List<RecordedCall> _calls;

        public CallRecordingConnection()
        {
            _calls = new List<RecordedCall>();
        }

        public void RecordCall(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            _calls.Add(new RecordedCall(arguments));
        }

        public Tuple<int, object, CancellationToken>[] ObsoleteRecordedCalls
        {
            get
            {
                return null; 
            }
        }

        public IReadOnlyCollection<RecordedCall> RecordedCalls => _calls;
    }
}