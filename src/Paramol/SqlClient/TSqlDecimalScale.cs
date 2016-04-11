namespace Paramol.SqlClient
{
    using System;

    /// <summary>
    ///     Represents the size of a <see cref="TSqlDecimalScale" />.
    /// </summary>
    public struct TSqlDecimalScale : IEquatable<TSqlDecimalScale>
    {
        private readonly byte _scale;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDecimalScale" /> struct.
        /// </summary>
        /// <param name="precision">The precision.</param>
        /// <param name="scale">The scale</param>
        /// <exception cref="System.ArgumentOutOfRangeException">scale;The scale must be between 0 and precision.</exception>
        public TSqlDecimalScale(byte precision, byte scale)
        {
            if (scale > precision)
                throw new ArgumentOutOfRangeException("scale", scale, string.Format("The value must be between 0 and {0}.", precision));
            _scale = scale;
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(TSqlDecimalScale other)
        {
            return _scale == other._scale;
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
            return obj is TSqlDecimalScale && Equals((TSqlDecimalScale)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _scale;
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlDecimalScale" /> are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> represent the same sql varchar size;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public static bool operator ==(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return left.Equals(right);
        }

        /// <summary>
        ///     Determines whether two specified instances of <see cref="TSqlDecimalScale" /> are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left" /> and <paramref name="right" /> do not represent the same sql varchar
        ///     size; otherwise, <c>false</c>.
        /// </returns>
        public static bool operator !=(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///     Converts a <see cref="TSqlDecimalScale" /> instance to an <see cref="Byte" />.
        /// </summary>
        /// <param name="size">The <see cref="TSqlDecimalScale" /> instance to convert.</param>
        /// <returns>The <see cref="Byte" /> size value.</returns>
        public static implicit operator byte(TSqlDecimalScale size)
        {
            return size._scale;
        }
    }
}