using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Represents a T-SQL DECIMAL parameter value.
    /// </summary>
    public class TSqlDecimalValue : IDbParameterValue
    {
        /// <summary>
        ///     The single instance of this value.
        /// </summary>
        public static readonly TSqlDecimalValue Instance = new TSqlDecimalValue();

        private readonly decimal _value;
        private readonly byte _precision;
        private readonly byte _scale;

        /// <summary>
        /// 
        /// </summary>
        public TSqlDecimalValue()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        public TSqlDecimalValue(decimal value, byte precision = 18, byte scale = 0)
        {
            _value = value;

            if (precision < 1 || precision > 38)
                throw new ArgumentOutOfRangeException("precision", precision, "The precision must be between 1 and 38.");

            _precision = precision;

            if (scale > precision)
                throw new ArgumentOutOfRangeException("scale", scale, "The scale must not be greater than the precision.");

            _scale = scale;
        }

        /// <summary>
        ///     Represents a T-SQL DECIMAL parameter value.
        /// </summary>
        public DbParameter ToDbParameter(string parameterName)
        {
            return ToSqlParameter(parameterName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public SqlParameter ToSqlParameter(string parameterName)
        {
            return new SqlParameter(
                parameterName,
                SqlDbType.Decimal,
                0,
                ParameterDirection.Input,
                false,
                _precision,
                _scale,
                "",
                DataRowVersion.Default,
                _value);
        }

        private bool Equals(TSqlDecimalValue other)
        {
            return _value == other._value 
                && _precision == other._precision 
                && _scale == other._scale;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TSqlDecimalValue) obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}