using System;
using System.Linq;

namespace Projac.Tests
{
    public struct RecordedCall : IEquatable<RecordedCall>
    {
        private readonly object[] _arguments;

        public RecordedCall(params object[] arguments)
        {
            _arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        public override bool Equals(object obj) => obj != null && obj.GetType() == typeof(RecordedCall) && Equals((RecordedCall)obj);
        public override int GetHashCode() => _arguments.Aggregate(19, (hashCode, current) => hashCode ^ current.GetHashCode());
        public bool Equals(RecordedCall other) => other._arguments.Length == _arguments.Length && _arguments.SequenceEqual(other._arguments);
    }
}