using System;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Paramol;
using Projac.Tests.Framework;

namespace Projac.Tests
{
    [TestFixture]
    public class SqlProjectionDescriptorBuilderTests
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
        public void DataDefinitionCommandsAreEmptyByDefault()
        {
            var sut = SutFactory();
            var result = sut.DataDefinitionCommands;
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void DataDefinitionCommandsCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.DataDefinitionCommands = null);
        }

        [Test]
        public void DataDefinitionCommandsCanBeEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.DataDefinitionCommands = new SqlNonQueryCommand[0]);
        }

        [Test]
        public void DataDefinitionCommandsCanBeNonEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.DataDefinitionCommands = new[] { CommandFactory(), CommandFactory() });
        }

        [Test]
        public void DataDefinitionCommandsArePreserved()
        {
            var sut = SutFactory();
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            sut.DataDefinitionCommands = new[] { command1, command2 };
            var result = sut.DataDefinitionCommands;
            Assert.That(result, Is.EquivalentTo(new[] { command1, command2 }));
        }

        [Test]
        public void ProjectionIsEmptyByDefault()
        {
            var sut = SutFactory();
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void ProjectionCanNotBeNull()
        {
            var sut = SutFactory();
            Assert.Throws<ArgumentNullException>(
                () => sut.Projection = null);
        }

        [Test]
        public void ProjectionCanBeEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.Projection = SqlProjection.Empty);
        }

        [Test]
        public void ProjectionCanBeNonEmpty()
        {
            var sut = SutFactory();
            Assert.DoesNotThrow(
                () => sut.Projection = new SqlProjection(
                    new[]
                    {
                        new SqlProjectionHandler(
                            typeof (object),
                            _ => new SqlNonQueryCommand[0])
                    }));
        }

        [Test]
        public void ProjectionEmptyIsPreserved()
        {
            var sut = SutFactory();
            sut.Projection = SqlProjection.Empty;
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(SqlProjection.Empty));
        }

        [Test]
        public void ProjectionIsPreserved()
        {
            var sut = SutFactory();
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            sut.Projection = projection;
            var result = sut.Projection;
            Assert.That(result, Is.SameAs(projection));
        }

        [Test]
        public void BuildReturnsExpectedResult()
        {
            var command1 = CommandFactory();
            var command2 = CommandFactory();
            var projection = new SqlProjection(
                new[]
                {
                    new SqlProjectionHandler(
                        typeof (object),
                        _ => new SqlNonQueryCommand[0])
                });
            var sut = new SqlProjectionDescriptorBuilder("identifier")
            {
                DataDefinitionCommands = new[] {command1, command2},
                Projection = projection
            };
            var result = sut.Build();
            Assert.That(result, Is.InstanceOf<SqlProjectionDescriptor>());
            Assert.That(result.Identifier, Is.EqualTo("identifier"));
            Assert.That(result.DataDefinitionCommands, Is.EquivalentTo(new[] { command1, command2 }));
            Assert.That(result.Projection, Is.SameAs(projection));
        }

        private static SqlProjectionDescriptorBuilder SutFactory(string identifier = Identifier)
        {
            return new SqlProjectionDescriptorBuilder(identifier);
        }

        private static SqlNonQueryCommand CommandFactory()
        {
            return new SqlNonQueryCommandStub("", new DbParameter[0], CommandType.Text);
        }
    }
}