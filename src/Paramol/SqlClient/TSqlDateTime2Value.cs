namespace Paramol.SqlClient
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// Represents the T-SQL DATETIME2 parameter value.
    /// </summary>
    public class TSqlDateTime2Value : IDbParameterValue
    {
        private readonly TSqlDateTime2Precision _precision;
        private readonly DateTime _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDateTime2Value" /> class.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <param name="precision">The parameter precision.</param>
        public TSqlDateTime2Value(DateTime value, TSqlDateTime2Precision precision)
        {
            _value = value;
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
                false,
                _precision,
                0,
                "",
                DataRowVersion.Default,
                _value);
        }

        private bool Equals(TSqlDateTime2Value other)
        {
            return (_value == other._value) && (_precision == other._precision);
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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TSqlDateTime2Value) obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode() ^ _precision;
        }
    }
}