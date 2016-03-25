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
        public static readonly IDbParameterValue AnsiStringNull = new SQLiteDbParameterValue(DbType.AnsiString, DBNull.Value);
        public static readonly IDbParameterValue AnsiStringFixedLengthNull = new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, DBNull.Value);
        public static readonly IDbParameterValue BinaryNull = new SQLiteDbParameterValue(DbType.Binary, DBNull.Value);
        public static readonly IDbParameterValue BooleanNull = new SQLiteDbParameterValue(DbType.Boolean, DBNull.Value);
        public static readonly IDbParameterValue ByteNull = new SQLiteDbParameterValue(DbType.Byte, DBNull.Value);
        public static readonly IDbParameterValue CurrencyNull = new SQLiteDbParameterValue(DbType.Currency, DBNull.Value);
        public static readonly IDbParameterValue DateNull = new SQLiteDbParameterValue(DbType.Date, DBNull.Value);
        public static readonly IDbParameterValue DateTimeNull = new SQLiteDbParameterValue(DbType.DateTime, DBNull.Value);
        public static readonly IDbParameterValue DateTime2Null = new SQLiteDbParameterValue(DbType.DateTime2, DBNull.Value);
        public static readonly IDbParameterValue DateTimeOffsetNull = new SQLiteDbParameterValue(DbType.DateTimeOffset, DBNull.Value);
        public static readonly IDbParameterValue DecimalNull = new SQLiteDbParameterValue(DbType.Decimal, DBNull.Value);
        public static readonly IDbParameterValue DoubleNull = new SQLiteDbParameterValue(DbType.Double, DBNull.Value);
        public static readonly IDbParameterValue GuidNull = new SQLiteDbParameterValue(DbType.Guid, DBNull.Value);
        public static readonly IDbParameterValue Int16Null = new SQLiteDbParameterValue(DbType.Int16, DBNull.Value);
        public static readonly IDbParameterValue Int32Null = new SQLiteDbParameterValue(DbType.Int32, DBNull.Value);
        public static readonly IDbParameterValue Int64Null = new SQLiteDbParameterValue(DbType.Int64, DBNull.Value);
        public static readonly IDbParameterValue ObjectNull = new SQLiteDbParameterValue(DbType.Object, DBNull.Value);
        public static readonly IDbParameterValue SByteNull = new SQLiteDbParameterValue(DbType.SByte, DBNull.Value);
        public static readonly IDbParameterValue SingleNull = new SQLiteDbParameterValue(DbType.Single, DBNull.Value);
        public static readonly IDbParameterValue StringNull = new SQLiteDbParameterValue(DbType.String, DBNull.Value);
        public static readonly IDbParameterValue StringFixedLengthNull = new SQLiteDbParameterValue(DbType.StringFixedLength, DBNull.Value);
        public static readonly IDbParameterValue TimeNull = new SQLiteDbParameterValue(DbType.Time, DBNull.Value);
        public static readonly IDbParameterValue UInt16Null = new SQLiteDbParameterValue(DbType.UInt16, DBNull.Value);
        public static readonly IDbParameterValue UInt32Null = new SQLiteDbParameterValue(DbType.UInt32, DBNull.Value);
        public static readonly IDbParameterValue UInt64Null = new SQLiteDbParameterValue(DbType.UInt64, DBNull.Value);
        public static readonly IDbParameterValue VarNumericNull = new SQLiteDbParameterValue(DbType.VarNumeric, DBNull.Value);
        public static readonly IDbParameterValue XmlNull = new SQLiteDbParameterValue(DbType.Xml, DBNull.Value);

        public static readonly Func<object, SQLiteDbParameterValue> AnsiStringFactory = value => new SQLiteDbParameterValue(DbType.AnsiString, value);
        public static readonly Func<object, SQLiteDbParameterValue> AnsiStringFixedLengthFactory = value => new SQLiteDbParameterValue(DbType.AnsiStringFixedLength, value);
        public static readonly Func<object, SQLiteDbParameterValue> BinaryFactory = value => new SQLiteDbParameterValue(DbType.Binary, value);
        public static readonly Func<object, SQLiteDbParameterValue> BooleanFactory = value => new SQLiteDbParameterValue(DbType.Boolean, value);
        public static readonly Func<object, SQLiteDbParameterValue> ByteFactory = value => new SQLiteDbParameterValue(DbType.Byte, value);
        public static readonly Func<object, SQLiteDbParameterValue> CurrencyFactory = value => new SQLiteDbParameterValue(DbType.Currency, value);
        public static readonly Func<object, SQLiteDbParameterValue> DateFactory = value => new SQLiteDbParameterValue(DbType.Date, value);
        public static readonly Func<object, SQLiteDbParameterValue> DateTimeFactory = value => new SQLiteDbParameterValue(DbType.DateTime, value);
        public static readonly Func<object, SQLiteDbParameterValue> DateTime2Factory = value => new SQLiteDbParameterValue(DbType.DateTime2, value);
        public static readonly Func<object, SQLiteDbParameterValue> DateTimeOffsetFactory = value => new SQLiteDbParameterValue(DbType.DateTimeOffset, value);
        public static readonly Func<object, SQLiteDbParameterValue> DecimalFactory = value => new SQLiteDbParameterValue(DbType.Decimal, value);
        public static readonly Func<object, SQLiteDbParameterValue> DoubleFactory = value => new SQLiteDbParameterValue(DbType.Double, value);
        public static readonly Func<object, SQLiteDbParameterValue> GuidFactory = value => new SQLiteDbParameterValue(DbType.Guid, value);
        public static readonly Func<object, SQLiteDbParameterValue> Int16Factory = value => new SQLiteDbParameterValue(DbType.Int16, value);
        public static readonly Func<object, SQLiteDbParameterValue> Int32Factory = value => new SQLiteDbParameterValue(DbType.Int32, value);
        public static readonly Func<object, SQLiteDbParameterValue> Int64Factory = value => new SQLiteDbParameterValue(DbType.Int64, value);
        public static readonly Func<object, SQLiteDbParameterValue> ObjectFactory = value => new SQLiteDbParameterValue(DbType.Object, value);
        public static readonly Func<object, SQLiteDbParameterValue> SByteFactory = value => new SQLiteDbParameterValue(DbType.SByte, value);
        public static readonly Func<object, SQLiteDbParameterValue> SingleFactory = value => new SQLiteDbParameterValue(DbType.Single, value);
        public static readonly Func<object, SQLiteDbParameterValue> StringFactory = value => new SQLiteDbParameterValue(DbType.String, value);
        public static readonly Func<object, SQLiteDbParameterValue> StringFixedLengthFactory = value => new SQLiteDbParameterValue(DbType.StringFixedLength, value);
        public static readonly Func<object, SQLiteDbParameterValue> TimeFactory = value => new SQLiteDbParameterValue(DbType.Time, value);
        public static readonly Func<object, SQLiteDbParameterValue> UInt16Factory = value => new SQLiteDbParameterValue(DbType.UInt16, value);
        public static readonly Func<object, SQLiteDbParameterValue> UInt32Factory = value => new SQLiteDbParameterValue(DbType.UInt32, value);
        public static readonly Func<object, SQLiteDbParameterValue> UInt64Factory = value => new SQLiteDbParameterValue(DbType.UInt64, value);
        public static readonly Func<object, SQLiteDbParameterValue> VarNumericFactory = value => new SQLiteDbParameterValue(DbType.VarNumeric, value);
        public static readonly Func<object, SQLiteDbParameterValue> XmlFactory = value => new SQLiteDbParameterValue(DbType.Xml, value);

        private readonly DbType _dbType;
        private readonly object _value;

        private SQLiteDbParameterValue(DbType dbType, object value)
        {
            _dbType = dbType;
            _value = value;
        }

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
    }
}