using System;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Paramol;
using Projac.Tests.Framework;

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
        public void DataDefinitionCommandsCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(
                () => SutFactory((SqlNonQueryCommand[])null)
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
            var dataDefinitionCommands = new[]
            {
                CommandFactory(),
                CommandFactory()
            };
            var projection = new SqlProjection(new[] { new SqlProjectionHandler(typeof(object), _ => new SqlNonQueryCommand[0]) });
            var sut = SutFactory("identifier-v2", dataDefinitionCommands, projection);

            Assert.That(sut.Identifier, Is.EqualTo("identifier-v2"));
            Assert.That(sut.DataDefinitionCommands, Is.EquivalentTo(dataDefinitionCommands));
            Assert.That(sut.Projection, Is.EqualTo(projection));
        }

        private static SqlProjectionDescriptor SutFactory(string identifier)
        {
            return SutFactory(identifier, new SqlNonQueryCommand[0], new SqlProjection(new SqlProjectionHandler[0]));
        }

        private static SqlProjectionDescriptor SutFactory(SqlNonQueryCommand[] dataDefinitionCommands)
        {
            return SutFactory(Identifier, dataDefinitionCommands, new SqlProjection(new SqlProjectionHandler[0]));
        }

        private static SqlProjectionDescriptor SutFactory(SqlProjection projection)
        {
            return SutFactory(Identifier, new SqlNonQueryCommand[0], projection);
        }

        private static SqlProjectionDescriptor SutFactory(
            string identifier,
            SqlNonQueryCommand[] dataDefinitionCommands, 
            SqlProjection projection)
        {
            return new SqlProjectionDescriptor(identifier, dataDefinitionCommands, projection);
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("text", new DbParameter[0], CommandType.Text);
        }
    }
}