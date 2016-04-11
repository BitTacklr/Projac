namespace Paramol.SqlClient
{
    using System;

    /// <summary>
    ///     Represents the size of a <see cref="TSqlDecimalPrecision" />.
    /// </summary>
    public struct TSqlDecimalPrecision : IEquatable<TSqlDecimalPrecision>
    {
        /// <summary>
        ///     Represents the maximum size value.
        /// </summary>
        public static readonly TSqlDecimalPrecision Max = new TSqlDecimalPrecision(38);

        /// <summary>
        ///     Represents the minimum size value.
        /// </summary>
        public static readonly TSqlDecimalPrecision Min = new TSqlDecimalPrecision(1);

        private readonly byte _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDecimalPrecision" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The value must be between 1 and 38.</exception>
        public TSqlDecimalPrecision(byte value)
        {
            if (value < 1 || value > 38)
                throw new ArgumentOutOfRangeException("value", value, "The value must be between 1 and 38.");
            _value = value;
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TSqlDecimalPrecision other)
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
            return obj is TSqlDecimalPrecision && Equals((TSqlDecimalPrecision)obj);
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
        ///     Determines whether two specified instances of <see cref="TSqlDecimalPrecision" /> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same sql varchar size;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(TSqlDecimalPrecision left, TSqlDecimalPrecision right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlDecimalPrecision" /> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same sql varchar
        ///     size; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(TSqlDecimalPrecision left, TSqlDecimalPrecision right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Converts a <see cref="TSqlDecimalPrecision" /> instance to an <see cref="Byte" />.
        /// </summary>
        /// <param name="size">The <see cref="TSqlDecimalPrecision" /> instance to convert.</param>
        /// <returns>The <see cref="Byte" /> size value.</returns>
        public static implicit operator byte(TSqlDecimalPrecision size)
        {
            return size._value;
        }

        /// <summary>
        ///     Converts an <see cref="Byte" /> to a <see cref="TSqlDecimalPrecision" /> instance.
        /// </summary>
        /// <param name="value">The <see cref="Byte" /> to convert.</param>
        /// <returns>The <see cref="TSqlDecimalPrecision" /> instance.</returns>
        public static implicit operator TSqlDecimalPrecision(byte value)
        {
            return new TSqlDecimalPrecision(value);
        }
    }
}