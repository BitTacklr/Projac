using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NUnit.Framework;
using Projac.Testing;
using Projac.Tests.Framework;

namespace Projac.Tests.Testing
{
    [TestFixture]
    public class ExpectRowCountExpectStateTests : ExpectStateFixture
    {
        protected override IScenarioExpectStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).GivenNone().When(new object()).ExpectRowCount(TSql.Query(""), 0);
        }
    }

    [TestFixture]
    public class ExpectEmptyResultSetExpectStateTests : ExpectStateFixture
    {
        protected override IScenarioExpectStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).GivenNone().When(new object()).ExpectEmptyResultSet(TSql.Query(""));
        }
    }

    [TestFixture]
    public class ExpectNonEmptyResultSetExpectStateTests : ExpectStateFixture
    {
        protected override IScenarioExpectStateBuilder SutFactory()
        {
            return new Scenario(TSqlProjection.Empty).GivenNone().When(new object()).ExpectNonEmptyResultSet(TSql.Query(""));
        }
    }

    public abstract class ExpectStateFixture
    {
        private IScenarioExpectStateBuilder _sut;

        protected abstract IScenarioExpectStateBuilder SutFactory();

        [SetUp]
        public void SetUp()
        {
            _sut = SutFactory();
        }

        [Test]
        public void ExpectRowCountQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectRowCount(null, 0));
        }

        [Test]
        public void ExpectNonEmptyResultQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectNonEmptyResultSet(null));
        }

        [Test]
        public void ExpectEmptyResultQueryCanNotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.ExpectEmptyResultSet(null));
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectRowCountIsPreservedUponBuildWithExpectedPassBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var random = new Random();
                    var id = random.Next();
                    var value = new string('a', random.Next(10));
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [Test] ([Id],[Value]) VALUES(@Id, @Value)";
                        command.Parameters.Add(TSql.Int(id).ToSqlParameter("@Id"));
                        command.Parameters.Add(TSql.VarChar(value, 10).ToSqlParameter("@Value"));
                        command.ExecuteNonQuery();
                    }
                    var expectation =
                        _sut.ExpectRowCount(
                            TSql.QueryFormat("SELECT COUNT(*) FROM [Test] WHERE [Id] = {0} AND [Value] = {1}",
                                TSql.Int(id),
                                TSql.VarChar(value, 10)), 1).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.True);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.False);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectRowCountIsPreservedUponBuildWithExpectedFailBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var random = new Random();
                    var id = random.Next();
                    var value = new string('a', random.Next(10));
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [Test] ([Id],[Value]) VALUES(@Id, @Value)";
                        command.Parameters.Add(TSql.Int(id).ToSqlParameter("@Id"));
                        command.Parameters.Add(TSql.VarChar(value, 10).ToSqlParameter("@Value"));
                        command.ExecuteNonQuery();
                    }
                    var expectation =
                        _sut.ExpectRowCount(
                            TSql.QueryFormat("SELECT COUNT(*) FROM [Test] WHERE [Id] = {0} AND [Value] = {1}",
                                TSql.Int(id),
                                TSql.VarChar(value, 10)), 0).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.False);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.True);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectEmptyResultSetIsPreservedUponBuildWithExpectedPassBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var expectation =
                        _sut.ExpectEmptyResultSet(
                            TSql.Query("SELECT * FROM [Test]")).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.True);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.False);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }   
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectEmptyResultSetIsPreservedUponBuildWithExpectedFailBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var random = new Random();
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [Test] ([Id],[Value]) VALUES (@Id, @Value)";
                        command.Parameters.Add(TSql.Int(random.Next()).ToSqlParameter("@Id"));
                        command.Parameters.Add(TSql.VarChar(new string('a', random.Next(10)), 10).ToSqlParameter("@Value"));
                        command.ExecuteNonQuery();
                    }
                    var expectation =
                        _sut.ExpectEmptyResultSet(
                            TSql.Query("SELECT * FROM [Test]")).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.False);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.True);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectNonEmptyResultSetIsPreservedUponBuildWithExpectedPassBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var random = new Random();
                    using (var command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO [Test] ([Id],[Value]) VALUES (@Id, @Value)";
                        command.Parameters.Add(TSql.Int(random.Next()).ToSqlParameter("@Id"));
                        command.Parameters.Add(TSql.VarChar(new string('a', random.Next(10)), 10).ToSqlParameter("@Value"));
                        command.ExecuteNonQuery();
                    }
                    var expectation =
                        _sut.ExpectNonEmptyResultSet(
                            TSql.QueryFormat("SELECT * FROM [Test]")).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.True);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.False);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [Test]
        [RequiresSqlServer]
        public void ExpectNonEmptyResultSetIsPreservedUponBuildWithExpectedFailBehavior()
        {
            using (var connection = TestDatabase.OpenConnection())
            {
                try
                {
                    //Arrange
                    SetUpTestSchema(connection);
                    var expectation =
                        _sut.ExpectNonEmptyResultSet(
                            TSql.QueryFormat("SELECT * FROM [Test]")).
                            Build().
                            Expectations.
                            Last();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            //Act
                            var result = expectation.Verify(transaction);

                            //Assert
                            Assert.That(result.Passed, Is.False);
                            Assert.That(result.Expectation, Is.EqualTo(expectation));
                            Assert.That(result.Failed, Is.True);
                        }
                        finally
                        {
                            transaction.Rollback();
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private static void SetUpTestSchema(SqlConnection connection)
        {
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Test')) BEGIN CREATE TABLE [Test] ([Id] INT PRIMARY KEY, [Value] VARCHAR(10)) END";
                command.ExecuteNonQuery();
                command.CommandText = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Test')) BEGIN DELETE FROM [Test] END";
                command.ExecuteNonQuery();
            }
        }
    }
}
