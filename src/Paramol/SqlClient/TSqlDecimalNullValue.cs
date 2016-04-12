namespace Paramol.SqlClient
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    ///     Represents the T-SQL DECIMAL NULL parameter value.
    /// </summary>
    public class TSqlDecimalNullValue : IDbParameterValue
    {
        private readonly TSqlDecimalScale _scale;
        private readonly TSqlDecimalPrecision _precision;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TSqlDecimalNullValue" /> class.
        /// </summary>
        /// <param name="precision">The maximum total number of decimal digits that will be stored, both to the left and to the right of the decimal point. The precision must be a value from 1 through the maximum precision of 38. The default precision is 18.</param>
        /// <param name="scale">The maximum number of decimal digits that can be stored to the right of the decimal point. Scale must be a value from 0 through p. The default scale is 0.</param>
        public TSqlDecimalNullValue(TSqlDecimalPrecision precision, TSqlDecimalScale scale)
        {
            if (scale > precision)
            {
                throw new ArgumentOutOfRangeException("scale", scale,
                    string.Format("The scale ({0}) must be less than or equal to the precision ({1}).", scale, precision));
            }

            _precision = precision;
            _scale = scale;
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
                SqlDbType.Decimal,
                0,
                ParameterDirection.Input,
                true,
                _precision,
                _scale,
                "",
                DataRowVersion.Default,
                DBNull.Value);
        }

        private bool Equals(TSqlDecimalNullValue other)
        {
            return _precision.Equals(other._precision) &&
                _scale.Equals(other._scale);
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

            return Equals((TSqlDecimalNullValue)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _precision.GetHashCode() ^ _scale.GetHashCode();
        }
    }
}