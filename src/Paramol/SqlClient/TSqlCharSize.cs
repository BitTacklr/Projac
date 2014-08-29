using System;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Represents the size of a <see cref="TSqlCharValue" />.
    /// </summary>
    public struct TSqlCharSize : IEquatable<TSqlCharSize>
    {
        /// <summary>
        ///     Represents the maximum size value.
        /// </summary>
        public static readonly TSqlCharSize Max = new TSqlCharSize(-1);

        private readonly int _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlCharSize" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The value must be between -1 and 8000.</exception>
        public TSqlCharSize(int value)
        {
            if (value < -1 || value > Limits.MaxAnsiSize)
                throw new ArgumentOutOfRangeException("value", value,
                    string.Format("The value must be between -1 and {0}.", Limits.MaxAnsiSize));
            _value = value;
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TSqlCharSize other)
        {
            return _value == other._value;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TSqlCharSize && Equals((TSqlCharSize) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _value;
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlCharSize" /> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same sql varchar size;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(TSqlCharSize left, TSqlCharSize right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlCharSize" /> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same sql varchar
        ///     size; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(TSqlCharSize left, TSqlCharSize right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Converts a <see cref="TSqlCharSize" /> instance to an <see cref="Int32" />.
        /// </summary>
        /// <param name="size">The <see cref="TSqlCharSize" /> instance to convert.</param>
        /// <returns>The <see cref="Int32" /> size value.</returns>
        public static implicit operator int(TSqlCharSize size)
        {
            return size._value;
        }

        /// <summary>
        ///     Converts an <see cref="Int32" /> to a <see cref="TSqlCharSize" /> instance.
        /// </summary>
        /// <param name="value">The <see cref="Int32" /> to convert.</param>
        /// <returns>The <see cref="TSqlCharSize" /> instance.</returns>
        public static implicit operator TSqlCharSize(int value)
        {
            return new TSqlCharSize(value);
        }
    }
}