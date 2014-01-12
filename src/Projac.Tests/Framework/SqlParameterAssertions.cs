using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;

namespace Projac.Tests.Framework
{
    internal static class SqlParameterAssertions
    {
        public static void Expect(this SqlParameter parameter, 
            string name, 
            SqlDbType sqlDbType, 
            object value,
            bool nullable = false, 
            int size = 0,
            int precision = 0, 
            int scale = 0)
        {
            Assert.That(parameter, Is.Not.Null);
                        
            Assert.That(parameter.ParameterName, Is.EqualTo(name));
            Assert.That(parameter.Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(parameter.LocaleId, Is.EqualTo(0));
                        
            Assert.That(parameter.Value, Is.EqualTo(value));
            Assert.That(parameter.IsNullable, Is.EqualTo(nullable));
            Assert.That(parameter.SqlDbType, Is.EqualTo(sqlDbType));
            Assert.That(parameter.Size, Is.EqualTo(size));
            Assert.That(parameter.Precision, Is.EqualTo(precision));
            Assert.That(parameter.Scale, Is.EqualTo(scale));
            Assert.That(parameter.Offset, Is.EqualTo(0));
                        
            Assert.That(parameter.SourceColumn, Is.EqualTo(""));
            Assert.That(parameter.SourceColumnNullMapping, Is.False);
            Assert.That(parameter.SourceVersion, Is.EqualTo(DataRowVersion.Default));
                        
            Assert.That(parameter.XmlSchemaCollectionDatabase, Is.EqualTo(""));
            Assert.That(parameter.XmlSchemaCollectionName, Is.EqualTo(""));
            Assert.That(parameter.XmlSchemaCollectionOwningSchema, Is.EqualTo(""));
        }
    }
}