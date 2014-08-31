using System;
using System.Data.Common;
using NUnit.Framework;
using Paramol;
using Paramol.SqlClient;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionDescriptorTests
    {
        private const string Identifier = "identifier:v1";

        [Test]
        public void IdentifierCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((string)null)
                );
        }

        [Test]
        public void DataDefinitionStatementsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((SqlNonQueryStatement[])null)
                );
        }

        [Test]
        public void ProjectionCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((SqlProjection)null)
                );
        }

        [Test]
        public void PropertiesArePreserved()
        {
            var dataDefinitionStatements = new[]
            {
                StatementFactory(),
                StatementFactory()
            };
            var projection = new SqlProjection(new[] {new SqlProjectionHandler(typeof (object), _ => new SqlNonQueryStatement[0])});
            var sut = SutFactory("identifier-v2", dataDefinitionStatements, projection);

            Assert.That(sut.Identifier, Is.EqualTo("identifier-v2"));
            Assert.That(sut.DataDefinitionStatements, Is.EquivalentTo(dataDefinitionStatements));
            Assert.That(sut.Projection, Is.EqualTo(projection));
        }

        private static SqlProjectionDescriptor SutFactory(string identifier)
        {
            return SutFactory(identifier, new SqlNonQueryStatement[0], new SqlProjection(new SqlProjectionHandler[0]));
        }

        private static SqlProjectionDescriptor SutFactory(SqlNonQueryStatement[] dataDefinitionStatements)
        {
            return SutFactory(Identifier, dataDefinitionStatements, new SqlProjection(new SqlProjectionHandler[0]));
        }

        private static SqlProjectionDescriptor SutFactory(SqlProjection projection)
        {
            return SutFactory(Identifier, new SqlNonQueryStatement[0], projection);
        }

        private static SqlProjectionDescriptor SutFactory(
            string identifier,
            SqlNonQueryStatement[] dataDefinitionStatements, 
            SqlProjection projection)
        {
            return new SqlProjectionDescriptor(identifier, dataDefinitionStatements, projection);
        }

        private static SqlNonQueryStatement StatementFactory()
        {
            return new SqlNonQueryStatement("text", new DbParameter[0]);
        }
    }
}