using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using NUnit.Framework;

namespace Paramol.Tests.SQLite
{
    internal static class SQLiteParameterAssertions
    {
        public static void ExpectSQLiteParameter(this DbParameter parameter,
            string name,
            DbType sqlDbType,
            object value,
            bool nullable = false,
            int size = 0)
        {
            Assert.That(parameter, Is.Not.Null);
            Assert.That(parameter, Is.InstanceOf<SQLiteParameter>());

            ((SQLiteParameter)parameter).ExpectSQLiteParameter(name, sqlDbType, value, nullable, size);
        }

        public static void ExpectSQLiteParameter(this SQLiteParameter parameter,
            string name,
            DbType dbType,
            object value,
            bool nullable = false,
            int size = 0)
        {
            Assert.That(parameter, Is.Not.Null);

            Assert.That(parameter.ParameterName, Is.EqualTo(name));
            Assert.That(parameter.Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(parameter.SourceColumn, Is.EqualTo(""));
            Assert.That(parameter.SourceColumnNullMapping, Is.False);
            Assert.That(parameter.SourceVersion, Is.EqualTo(DataRowVersion.Default));
            Assert.That(parameter.Value, Is.EqualTo(value));
            Assert.That(parameter.Size, Is.EqualTo(size));
            Assert.That(parameter.IsNullable, Is.EqualTo(nullable));
            Assert.That(parameter.DbType, Is.EqualTo(dbType));
        }
    }
}