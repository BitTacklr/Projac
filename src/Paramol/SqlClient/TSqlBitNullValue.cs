using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Represents the T-SQL BIT NULL parameter value.
    /// </summary>
    public class TSqlBitNullValue : IDbParameterValue
    {
        /// <summary>
        ///     The single instance of this value.
        /// </summary>
        public static readonly TSqlBitNullValue Instance = new TSqlBitNullValue();

        private TSqlBitNullValue()
        {
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
            return new SqlParameter(
                parameterName,
                SqlDbType.Bit,
                1,
                ParameterDirection.Input,
                true,
                0,
                0,
                "",
                DataRowVersion.Default,
                DBNull.Value);
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
            return new SqlParameter(
                parameterName,
                SqlDbType.Bit,
                1,
                ParameterDirection.Input,
                true,
                0,
                0,
                "",
                DataRowVersion.Default,
                DBNull.Value);
        }

        private static bool Equals(TSqlBitNullValue value)
        {
            return ReferenceEquals(value, Instance);
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
            return Equals((TSqlBitNullValue) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return 0;
        }
    }
}