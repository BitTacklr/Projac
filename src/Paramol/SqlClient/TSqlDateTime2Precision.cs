namespace Paramol.SqlClient
{
    using System;

    /// <summary>
    ///     Represents the precision of a <see cref="TSqlDateTime2Value" />.
    /// </summary>
    public struct TSqlDateTime2Precision : IEquatable<TSqlDateTime2Precision>
    {
        /// <summary>
        ///     Represents the maximum precision value.
        /// </summary>
        public static readonly TSqlDateTime2Precision Max = new TSqlDateTime2Precision(7);

        /// <summary>
        ///     Represents the minimum precision value.
        /// </summary>
        public static readonly TSqlDateTime2Precision Min = new TSqlDateTime2Precision(0);

        /// <summary>
        ///     Represents the default precision value.
        /// </summary>
        public static readonly TSqlDateTime2Precision Default = new TSqlDateTime2Precision(7);

        private readonly byte _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDateTime2Precision" /> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">value;The value must be between 0 and 7.</exception>
        public TSqlDateTime2Precision(byte value)
        {
            if (value > 7)
            {
                throw new ArgumentOutOfRangeException("value", value,
                    string.Format("The value must be between {0} and {1}.", 0, 7));
            }

            _value = value;
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TSqlDateTime2Precision other)
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
            return obj is TSqlDateTime2Precision && Equals((TSqlDateTime2Precision)obj);
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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlDateTime2Precision" /> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same sql datetime2 precision;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(TSqlDateTime2Precision left, TSqlDateTime2Precision right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlDateTime2Precision" /> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same sql datetime2
        ///     precision; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(TSqlDateTime2Precision left, TSqlDateTime2Precision right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Converts a <see cref="TSqlDateTime2Precision" /> instance to a <see cref="byte" />.
        /// </summary>
        /// <param name="precision">The <see cref="TSqlDateTime2Precision" /> instance to convert.</param>
        /// <returns>The <see cref="byte" /> size value.</returns>
        public static implicit operator byte(TSqlDateTime2Precision precision)
        {
            return precision._value;
        }

        /// <summary>
        ///     Converts a <see cref="byte" /> to a <see cref="TSqlDateTime2Precision" /> instance.
        /// </summary>
        /// <param name="value">The <see cref="byte" /> to convert.</param>
        /// <returns>The <see cref="TSqlDateTime2Precision" /> instance.</returns>
        public static implicit operator TSqlDateTime2Precision(byte value)
        {
            return new TSqlDateTime2Precision(value);
        }
    }
}