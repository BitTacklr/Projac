using System;
using System.Collections.Generic;
using System.Threading;

namespace Projac.Tests
{
    public class CallRecordingConnection
    {
        private readonly List<Tuple<int, object, CancellationToken>> _obsoleteCalls;
        private readonly List<object[]> _calls;

        public CallRecordingConnection()
        {
            _obsoleteCalls = new List<Tuple<int, object, CancellationToken>>();
            _calls = new List<object[]>();
        }

        public void RecordCall(int handler, object message, CancellationToken token)
        {
            _obsoleteCalls.Add(new Tuple<int, object, CancellationToken>(handler, message, token));
        }

        public void RecordCall(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            _calls.Add(arguments);
        }

        public Tuple<int, object, CancellationToken>[] ObsoleteRecordedCalls
        {
            get
            {
                return _obsoleteCalls.ToArray(); 
            }
        }

        public IReadOnlyCollection<object[]> RecordedCalls => _calls;
    }
}