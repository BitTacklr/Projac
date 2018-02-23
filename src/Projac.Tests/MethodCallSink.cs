using System;
using System.Threading;
using NUnit.Framework.Constraints;

namespace Projac
{
    public class MethodCallSink
    {
        public MethodCallSink()
        {
            WasCaptured = false;
        }

        public bool WasCaptured { get; private set; }
        public object[] Arguments { get; private set; }
        
        public void Capture(params object[] arguments)
        {
            Arguments = arguments ?? throw new System.ArgumentNullException(nameof(arguments));
            WasCaptured = true;
        }

        public bool Matches(params object[] arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));
            if (!WasCaptured) return false;
            if (arguments.Length != Arguments.Length) return false;
            for(var index = 0; index < arguments.Length; index++)
            {
                if(!arguments[index].Equals(Arguments[index])) return false;
            }
            return true;
        }
    }
}