using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using Projac.Sql;

namespace Projac.SqlClient
{
    /// <summary>
    ///     Represents a T-SQL VARBINARY parameter value.
    /// </summary>
    public class TSqlVarBinaryValue : IDbParameterValue
    {
        private readonly int _size;
        private readonly byte[] _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlVarBinaryValue" /> class.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="size">The parameter size.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="value" /> is <c>null</c>.</exception>
        public TSqlVarBinaryValue(byte[] value, TSqlVarBinarySize size)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            _value = value;
            _size = size;
        }

        /// <summary>
        ///     Creates a <see cref="DbParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>
        ///     A <see cref="DbParameter" />.
        /// </returns>
        public DbParameter ToDbParameter(string parameterName)
        {
            return ToSqlParameter(parameterName);
        }

        /// <summary>
        ///     Creates a <see cref="SqlParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>
        ///     A <see cref="SqlParameter" />.
        /// </returns>
        public SqlParameter ToSqlParameter(string parameterName)
        {
#if NET46
            return new SqlParameter(
                parameterName,
                SqlDbType.VarBinary,
                _size,
                ParameterDirection.Input,
                false,
                0,
                0,
                "",
                DataRowVersion.Default,
                _value);
#elif NETSTANDARD2_0
            return new SqlParameter 
                {
                    ParameterName = parameterName,
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.VarBinary,
                    Size = _size,
                    Value = _value,
                    SourceColumn = "",
                    IsNullable = false,
                    Precision = 0,
                    Scale = 0,
                    SourceVersion = DataRowVersion.Default
                };
#endif
        }

        private bool Equals(TSqlVarBinaryValue other)
        {
            return _value.SequenceEqual(other._value) && _size == other._size;
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
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TSqlVarBinaryValue) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.Aggregate(_size, (aggregation, current) => aggregation ^ current);
        }
    }
}