using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Tests
{
    public class RecordedCallEqualityComparer : IEqualityComparer<object[]>
    {
        public bool Equals(object[] left, object[] right)
        {
            if (left == null && right == null) return true;
            if (left == null || right== null) return false;
            if (left.Length != right.Length) return false;
            return left.SequenceEqual(right);
        }

        public int GetHashCode(object[] instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            return instance.Aggregate(19, (hashCode, current) => hashCode ^ current.GetHashCode());
        }
    }
}