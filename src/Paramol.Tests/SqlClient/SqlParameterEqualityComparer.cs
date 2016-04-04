using System.Collections.Generic;
using System.Data.SqlClient;

namespace Paramol.Tests.SqlClient
{
    internal class SqlParameterEqualityComparer : IEqualityComparer<SqlParameter>
    {
        public bool Equals(SqlParameter x, SqlParameter y)
        {
            if (x == null && y == null) return true;
            if (x != null && y != null)
            {
                return Equals(x.ParameterName, y.ParameterName) &&
                       Equals(x.Direction, y.Direction)  &&
                       Equals(x.LocaleId, y.LocaleId)  &&
                       Equals(x.Value, y.Value)  &&
                       Equals(x.IsNullable, y.IsNullable)  &&
                       Equals(x.SqlDbType, y.SqlDbType)  &&
                       Equals(x.Size, y.Size)  &&
                       Equals(x.Precision, y.Precision)  &&
                       Equals(x.Scale, y.Scale)  &&
                       Equals(x.Offset, y.Offset)  &&
                       Equals(x.SourceColumn, y.SourceColumn)  &&
                       Equals(x.SourceColumnNullMapping, y.SourceColumnNullMapping)  &&
                       Equals(x.SourceVersion, y.SourceVersion)  &&
                       Equals(x.XmlSchemaCollectionDatabase, y.XmlSchemaCollectionDatabase)  &&
                       Equals(x.XmlSchemaCollectionName, y.XmlSchemaCollectionName)  &&
                       Equals(x.XmlSchemaCollectionOwningSchema, y.XmlSchemaCollectionOwningSchema);
            }
            return false;
        }

        public int GetHashCode(SqlParameter obj)
        {
            if (obj == null)
                return 0;
            return obj.ParameterName.GetHashCode() ^ 
                   obj.Direction.GetHashCode() ^
                   obj.LocaleId.GetHashCode() ^
                   (obj.Value == null ? 0 : obj.Value.GetHashCode()) ^
                   obj.IsNullable.GetHashCode() ^
                   obj.SqlDbType.GetHashCode() ^
                   obj.Size.GetHashCode() ^
                   obj.Precision.GetHashCode() ^
                   obj.Scale.GetHashCode() ^
                   obj.Offset.GetHashCode() ^
                   obj.SourceColumn.GetHashCode() ^
                   obj.SourceColumnNullMapping.GetHashCode() ^
                   obj.SourceVersion.GetHashCode() ^
                   obj.XmlSchemaCollectionDatabase.GetHashCode() ^
                   obj.XmlSchemaCollectionName.GetHashCode() ^
                   obj.XmlSchemaCollectionOwningSchema.GetHashCode();
        }
    }
}