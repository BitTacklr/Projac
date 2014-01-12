using System;
using System.Data;
using System.Data.SqlClient;

namespace Projac
{
    /// <summary>
    /// Represents a T-SQL NVARCHAR parameter value.
    /// </summary>
    public class TSqlNVarCharValue : ITSqlParameterValue
    {
        private readonly string _value;
        private readonly int _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlNVarCharValue"/> class.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="value"/> is <c>null</c>.</exception>
        public TSqlNVarCharValue(string value, TSqlNVarCharSize size)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            _value = value;
            _size = size;
        }

        /// <summary>
        /// Creates a <see cref="SqlParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>
        /// A <see cref="SqlParameter" />.
        /// </returns>
        public SqlParameter ToSqlParameter(string parameterName)
        {
            return new SqlParameter(
                parameterName,
                SqlDbType.VarChar,
                _size,
                ParameterDirection.Input,
                false,
                0,
                0,
                "",
                DataRowVersion.Default,
                _value);
        }

        bool Equals(TSqlNVarCharValue other)
        {
            return string.Equals(_value, other._value) && _size == other._size;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TSqlNVarCharValue)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode() ^ _size;
        }
    }
}