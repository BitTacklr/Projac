namespace Paramol.SqlClient
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// Represents the T-SQL DATETIME2 NULL parameter value.
    /// </summary>
    public class TSqlDateTime2NullValue : IDbParameterValue
    {
        private readonly TSqlDateTime2Precision _precision;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDateTime2NullValue" /> class.
        /// </summary>
        /// <param name="precision">The parameter precision.</param>
        public TSqlDateTime2NullValue(TSqlDateTime2Precision precision)
        {
            _precision = precision;
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
            return new SqlParameter(
                parameterName,
                SqlDbType.DateTime2,
                8,
                ParameterDirection.Input,
                true,
                _precision,
                0,
                "",
                DataRowVersion.Default,
                DBNull.Value);
        }

        private bool Equals(TSqlDateTime2NullValue value)
        {
            return value._precision == _precision;
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
            {
                return false;
            }

            return Equals((TSqlDateTime2NullValue)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _precision.GetHashCode();
        }
    }
}