using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Projac.Sql;

namespace Projac.SqlClient
{
    /// <summary>
    ///     Represents the T-SQL BINARY NULL parameter value.
    /// </summary>
    public class TSqlBinaryNullValue : IDbParameterValue
    {
        private readonly TSqlBinarySize _size;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlBinaryNullValue" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public TSqlBinaryNullValue(TSqlBinarySize size)
        {
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
#if NET46 || NET452
            return new SqlParameter(
                parameterName,
                SqlDbType.Binary,
                _size,
                ParameterDirection.Input,
                true,
                0,
                0,
                "",
                DataRowVersion.Default,
                DBNull.Value);
#elif NETSTANDARD2_0
            return new SqlParameter 
                {
                    ParameterName = parameterName,
                    Direction = ParameterDirection.Input,
                    SqlDbType = SqlDbType.Binary,
                    Size = _size,
                    Value = DBNull.Value,
                    SourceColumn = "",
                    IsNullable = true,
                    Precision = 0,
                    Scale = 0,
                    SourceVersion = DataRowVersion.Default
                };
#endif
        }

        private bool Equals(TSqlBinaryNullValue value)
        {
            return value._size == _size;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;
            return Equals((TSqlBinaryNullValue) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _size.GetHashCode();
        }
    }
}