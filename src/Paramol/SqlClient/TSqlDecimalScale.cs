namespace Paramol.SqlClient
{
    using System;

    /// <summary>
    ///     Represents the scale of a <see cref="TSqlDecimalValue" />. The maximum number of decimal digits that can be stored to the right of the decimal point. Scale must be a value from 0 through the precision of the decimal value. The default scale is 0.
    /// </summary>
    public struct TSqlDecimalScale : IEquatable<TSqlDecimalScale>, IComparable<TSqlDecimalScale>, IComparable<TSqlDecimalPrecision>
    {
        /// <summary>
        ///     Represents the maximum precision value.
        /// </summary>
        public static readonly TSqlDecimalScale Max = new TSqlDecimalScale(38);

        /// <summary>
        ///     Represents the minimum precision value.
        /// </summary>
        public static readonly TSqlDecimalScale Min = new TSqlDecimalScale(0);

        /// <summary>
        ///     Represents the default value.
        /// </summary>
        public static readonly TSqlDecimalScale Default = new TSqlDecimalScale(0);

        private readonly byte _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDecimalScale" /> struct.
        /// </summary>
        /// <param name="value">The scale</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is not between 0 and 38.</exception>
        public TSqlDecimalScale(byte value)
        {
            if (value > 38)
            {
                throw new ArgumentOutOfRangeException("value", value,
                    string.Format("The value must be between {0} and {1}.", 0, 38));
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
        public bool Equals(TSqlDecimalScale other)
        {
            return _value == other._value;
        }

        /// <summary>
        /// Compares this instance to a <see cref="TSqlDecimalScale"/>.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other"/> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other"/>. Greater than zero This instance follows <paramref name="other"/> in the sort order.
        /// </returns>
        public int CompareTo(TSqlDecimalScale other)
        {
            return _value.CompareTo(other);
        }

        /// <summary>
        /// Compares this instance to a <see cref="TSqlDecimalPrecision"/>.
        /// </summary>
        /// <param name="other">The other instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other"/> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other"/>. Greater than zero This instance follows <paramref name="other"/> in the sort order.
        /// </returns>
        public int CompareTo(TSqlDecimalPrecision other)
        {
            return _value.CompareTo(other);
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
            return _value;
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
        /// <param name="scale">The <see cref="TSqlDecimalScale" /> instance to convert.</param>
        /// <returns>The <see cref="Byte" /> size value.</returns>
        public static implicit operator byte(TSqlDecimalScale scale)
        {
            return scale._value;
        }

        /// <summary>
        ///     Converts a <see cref="TSqlDecimalScale" /> instance to an <see cref="Byte" />.
        /// </summary>
        /// <param name="value">The <see cref="TSqlDecimalScale" /> instance to convert.</param>
        /// <returns>The <see cref="Byte" /> size value.</returns>
        public static implicit operator TSqlDecimalScale(byte value)
        {
            return new TSqlDecimalScale(value);
        }

        /// <summary>
        /// Compares two instances of <see cref="TSqlDecimalScale"/> using the less than or equal to operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator <=(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Compares two instances of <see cref="TSqlDecimalScale"/> using the less than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator <(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Compares two instances of <see cref="TSqlDecimalScale"/> using the greater than or equal to operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator >=(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Compares two instances of <see cref="TSqlDecimalScale"/> using the greater than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator >(TSqlDecimalScale left, TSqlDecimalScale right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Compares a <see cref="TSqlDecimalScale"/> to a <see cref="TSqlDecimalPrecision"/> using the less than or equal to operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator <=(TSqlDecimalScale left, TSqlDecimalPrecision right)
        {
            return left.CompareTo(right) <= 0;
        }

        /// <summary>
        /// Compares a <see cref="TSqlDecimalScale"/> to a <see cref="TSqlDecimalPrecision"/> using the less than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator <(TSqlDecimalScale left, TSqlDecimalPrecision right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Compares a <see cref="TSqlDecimalScale"/> to a <see cref="TSqlDecimalPrecision"/> using the greater than or equal to operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator >=(TSqlDecimalScale left, TSqlDecimalPrecision right)
        {
            return left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Compares a <see cref="TSqlDecimalScale"/> to a <see cref="TSqlDecimalPrecision"/> using the greater than operator.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>
        ///     <c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>, otherwise false.
        /// </returns>
        public static bool operator >(TSqlDecimalScale left, TSqlDecimalPrecision right)
        {
            return left.CompareTo(right) > 0;
        }
    }
}