using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Projac.Sql;

namespace Projac.SqlClient
{
    /// <summary>
    ///     Represents the T-SQL VARBINARY NULL parameter value.
    /// </summary>
    public class TSqlVarBinaryNullValue : IDbParameterValue
    {
        private readonly TSqlVarBinarySize _size;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlVarBinaryNullValue" /> class.
        /// </summary>
        /// <param name="size">The size.</param>
        public TSqlVarBinaryNullValue(TSqlVarBinarySize size)
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
#if NET46
            return new SqlParameter(
                parameterName,
                SqlDbType.VarBinary,
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
                    SqlDbType = SqlDbType.VarBinary,
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

        private bool Equals(TSqlVarBinaryNullValue value)
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
            return Equals((TSqlVarBinaryNullValue) obj);
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