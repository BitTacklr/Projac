using System.Collections.Generic;
using System.Data.SQLite;

namespace Paramol.Tests.SQLite
{
    internal class SQLiteParameterEqualityComparer : IEqualityComparer<SQLiteParameter>
    {
        public bool Equals(SQLiteParameter x, SQLiteParameter y)
        {
            if (x == null && y == null) return true;
            if (x != null && y != null)
            {
                return Equals(x.ParameterName, y.ParameterName) &&
                       Equals(x.Direction, y.Direction) &&
                       Equals(x.Value, y.Value) &&
                       Equals(x.IsNullable, y.IsNullable) &&
                       Equals(x.DbType, y.DbType) &&
                       Equals(x.Size, y.Size) &&
                       Equals(x.SourceColumn, y.SourceColumn) &&
                       Equals(x.SourceColumnNullMapping, y.SourceColumnNullMapping) &&
                       Equals(x.SourceVersion, y.SourceVersion);
            }
            return false;
        }

        public int GetHashCode(SQLiteParameter obj)
        {
            if (obj == null)
                return 0;
            return obj.ParameterName.GetHashCode() ^
                   obj.Direction.GetHashCode() ^
                   (obj.Value == null ? 0 : obj.Value.GetHashCode()) ^
                   obj.IsNullable.GetHashCode() ^
                   obj.DbType.GetHashCode() ^
                   obj.Size.GetHashCode() ^
                   obj.SourceColumn.GetHashCode() ^
                   obj.SourceColumnNullMapping.GetHashCode() ^
                   obj.SourceVersion.GetHashCode();
        }
    }
}