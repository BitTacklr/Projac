using System;
using System.Collections.Generic;
using System.Threading;

namespace Projac.Tests
{
    public class CallRecordingConnectionWithMetadata
    {
        private readonly List<Tuple<int, object, object, CancellationToken>> _calls;

        public CallRecordingConnectionWithMetadata()
        {
            _calls = new List<Tuple<int, object, object, CancellationToken>>();
        }

        public void RecordCall(int handler, object message, object metadata, CancellationToken token)
        {
            _calls.Add(new Tuple<int, object, object, CancellationToken>(handler, message, metadata, token));
        }

        public Tuple<int, object, object, CancellationToken>[] RecordedCalls
        {
            get
            {
                return _calls.ToArray(); 
            }
        }
    }
}