using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Paramol.SQLite
{
    /// <summary>
    ///     Represents a SQLite parameter value.
    /// </summary>
    public class SQLiteDbParameterValue : IDbParameterValue
    {
        /// <summary>
        ///     Represents the AnsiString NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue AnsiStringNull = new SQLiteDbParameterValue(DbType.AnsiString, DBNull.Value);
        /// <summary>
        ///     Represents the AnsiStringFixedLength NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue AnsiStringFixedLengthNull = new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, DBNull.Value);
        /// <summary>
        ///     Represents the Binary NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue BinaryNull = new SQLiteDbParameterValue(DbType.Binary, DBNull.Value);
        /// <summary>
        ///     Represents the Boolean NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue BooleanNull = new SQLiteDbParameterValue(DbType.Boolean, DBNull.Value);
        /// <summary>
        ///     Represents the Byte NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue ByteNull = new SQLiteDbParameterValue(DbType.Byte, DBNull.Value);
        /// <summary>
        ///     Represents the Currency NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue CurrencyNull = new SQLiteDbParameterValue(DbType.Currency, DBNull.Value);
        /// <summary>
        ///     Represents the Date NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DateNull = new SQLiteDbParameterValue(DbType.Date, DBNull.Value);
        /// <summary>
        ///     Represents the DateTime NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DateTimeNull = new SQLiteDbParameterValue(DbType.DateTime, DBNull.Value);
        /// <summary>
        ///     Represents the DateTime2 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DateTime2Null = new SQLiteDbParameterValue(DbType.DateTime2, DBNull.Value);
        /// <summary>
        ///     Represents the DateTimeOffset NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DateTimeOffsetNull = new SQLiteDbParameterValue(DbType.DateTimeOffset, DBNull.Value);
        /// <summary>
        ///     Represents the Decimal NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DecimalNull = new SQLiteDbParameterValue(DbType.Decimal, DBNull.Value);
        /// <summary>
        ///     Represents the Double NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue DoubleNull = new SQLiteDbParameterValue(DbType.Double, DBNull.Value);
        /// <summary>
        ///     Represents the Guid NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue GuidNull = new SQLiteDbParameterValue(DbType.Guid, DBNull.Value);
        /// <summary>
        ///     Represents the Int16 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue Int16Null = new SQLiteDbParameterValue(DbType.Int16, DBNull.Value);
        /// <summary>
        ///     Represents the Int32 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue Int32Null = new SQLiteDbParameterValue(DbType.Int32, DBNull.Value);
        /// <summary>
        ///     Represents the Int64 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue Int64Null = new SQLiteDbParameterValue(DbType.Int64, DBNull.Value);
        /// <summary>
        ///     Represents the Object NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue ObjectNull = new SQLiteDbParameterValue(DbType.Object, DBNull.Value);
        /// <summary>
        ///     Represents the SByte NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue SByteNull = new SQLiteDbParameterValue(DbType.SByte, DBNull.Value);
        /// <summary>
        ///     Represents the Single NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue SingleNull = new SQLiteDbParameterValue(DbType.Single, DBNull.Value);
        /// <summary>
        ///     Represents the String NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue StringNull = new SQLiteDbParameterValue(DbType.String, DBNull.Value);
        /// <summary>
        ///     Represents the StringFixedLength NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue StringFixedLengthNull = new SQLiteDbParameterValue(DbType.StringFixedLength, DBNull.Value);
        /// <summary>
        ///     Represents the Time NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue TimeNull = new SQLiteDbParameterValue(DbType.Time, DBNull.Value);
        /// <summary>
        ///     Represents the UInt16 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue UInt16Null = new SQLiteDbParameterValue(DbType.UInt16, DBNull.Value);
        /// <summary>
        ///     Represents the UInt32 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue UInt32Null = new SQLiteDbParameterValue(DbType.UInt32, DBNull.Value);
        /// <summary>
        ///     Represents the UInt64 NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue UInt64Null = new SQLiteDbParameterValue(DbType.UInt64, DBNull.Value);
        /// <summary>
        ///     Represents the VarNumeric NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue VarNumericNull = new SQLiteDbParameterValue(DbType.VarNumeric, DBNull.Value);
        /// <summary>
        ///     Represents the Xml NULL parameter value.
        /// </summary>
        public static readonly IDbParameterValue XmlNull = new SQLiteDbParameterValue(DbType.Xml, DBNull.Value);

        /// <summary>
        ///     Represents a AnsiString parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> AnsiStringFactory = value => new SQLiteDbParameterValue(DbType.AnsiString, value);
        /// <summary>
        ///     Represents a AnsiStringFixedLength parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> AnsiStringFixedLengthFactory = value => new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, value);
        /// <summary>
        ///     Represents a Binary parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> BinaryFactory = value => new SQLiteDbParameterValue(DbType.Binary, value);
        /// <summary>
        ///     Represents a Boolean parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> BooleanFactory = value => new SQLiteDbParameterValue(DbType.Boolean, value);
        /// <summary>
        ///     Represents a Byte parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> ByteFactory = value => new SQLiteDbParameterValue(DbType.Byte, value);
        /// <summary>
        ///     Represents a Currency parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> CurrencyFactory = value => new SQLiteDbParameterValue(DbType.Currency, value);
        /// <summary>
        ///     Represents a Date parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DateFactory = value => new SQLiteDbParameterValue(DbType.Date, value);
        /// <summary>
        ///     Represents a DateTime parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DateTimeFactory = value => new SQLiteDbParameterValue(DbType.DateTime, value);
        /// <summary>
        ///     Represents a DateTime2 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DateTime2Factory = value => new SQLiteDbParameterValue(DbType.DateTime2, value);
        /// <summary>
        ///     Represents a DateTimeOffset parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DateTimeOffsetFactory = value => new SQLiteDbParameterValue(DbType.DateTimeOffset, value);
        /// <summary>
        ///     Represents a Decimal parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DecimalFactory = value => new SQLiteDbParameterValue(DbType.Decimal, value);
        /// <summary>
        ///     Represents a Double parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> DoubleFactory = value => new SQLiteDbParameterValue(DbType.Double, value);
        /// <summary>
        ///     Represents a Guid parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> GuidFactory = value => new SQLiteDbParameterValue(DbType.Guid, value);
        /// <summary>
        ///     Represents a Int16 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> Int16Factory = value => new SQLiteDbParameterValue(DbType.Int16, value);
        /// <summary>
        ///     Represents a Int32 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> Int32Factory = value => new SQLiteDbParameterValue(DbType.Int32, value);
        /// <summary>
        ///     Represents a Int64 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> Int64Factory = value => new SQLiteDbParameterValue(DbType.Int64, value);
        /// <summary>
        ///     Represents a Object parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> ObjectFactory = value => new SQLiteDbParameterValue(DbType.Object, value);
        /// <summary>
        ///     Represents a SByte parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> SByteFactory = value => new SQLiteDbParameterValue(DbType.SByte, value);
        /// <summary>
        ///     Represents a Single parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> SingleFactory = value => new SQLiteDbParameterValue(DbType.Single, value);
        /// <summary>
        ///     Represents a String parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> StringFactory = value => new SQLiteDbParameterValue(DbType.String, value);
        /// <summary>
        ///     Represents a StringFixedLength parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> StringFixedLengthFactory = value => new SQLiteDbParameterValue(DbType.StringFixedLength, value);
        /// <summary>
        ///     Represents a Time parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> TimeFactory = value => new SQLiteDbParameterValue(DbType.Time, value);
        /// <summary>
        ///     Represents a UInt16 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> UInt16Factory = value => new SQLiteDbParameterValue(DbType.UInt16, value);
        /// <summary>
        ///     Represents a UInt32 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> UInt32Factory = value => new SQLiteDbParameterValue(DbType.UInt32, value);
        /// <summary>
        ///     Represents a UInt64 parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> UInt64Factory = value => new SQLiteDbParameterValue(DbType.UInt64, value);
        /// <summary>
        ///     Represents a VarNumeric parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> VarNumericFactory = value => new SQLiteDbParameterValue(DbType.VarNumeric, value);
        /// <summary>
        ///     Represents a Xml parameter value factory.
        /// </summary>
        public static readonly Func<object, SQLiteDbParameterValue> XmlFactory = value => new SQLiteDbParameterValue(DbType.Xml, value);

        private readonly DbType _dbType;
        private readonly object _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SQLiteDbParameterValue" /> class.
        /// </summary>
        /// <param name="dbType">The parameter database type.</param>
        /// <param name="value">The parameter value.</param>
        public SQLiteDbParameterValue(DbType dbType, object value)
        {
            _dbType = dbType;
            _value = value;
        }

        /// <summary>
        ///     Creates a <see cref="DbParameter" /> instance based on this instance.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>A <see cref="DbParameter" />.</returns>
        public DbParameter ToDbParameter(string parameterName)
        {
            return new SQLiteParameter(
                parameterName,
                _dbType,
                0,
                ParameterDirection.Input,
                true,
                0,
                0,
                "",
                DataRowVersion.Current,
                _value);
        }

        private bool Equals(SQLiteDbParameterValue other)
        {
            return 
                _dbType.Equals(other._dbType) && 
                Equals(_value, other._value);
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
            return Equals((SQLiteDbParameterValue)obj);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _dbType.GetHashCode() ^ (_value == null ? 0 : _value.GetHashCode());
        }
    }
}