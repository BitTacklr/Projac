using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using Paramol.SqlClient;

namespace Paramol.Tests.SQLite
{
    public class SQLiteSyntaxTestCases
    {
        private static readonly SqlClientSyntax Sql = new SqlClientSyntax();

        public static IEnumerable<TestCaseData> NonQueryStatementCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatement("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatement("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatement("text", new { }),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatement("text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatement("text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.Text));
        }

        public static IEnumerable<TestCaseData> NonQueryStatementIfCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(true, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(true, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(true, "text",
                    new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.NonQueryStatementIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(false, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(false, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementIf(false, "text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> NonQueryStatementUnlessCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(false, "text", new { }),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(false, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(false, "text",
                    new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(true, "text", new { }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(true, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementUnless(true, "text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlNonQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> NonQueryStatementFormatCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text"),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text", parameters: null),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text", new IDbParameterValue[0]),
                new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text {0}", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand("text @P0", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0"),
                    new SQLiteParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.NonQueryStatementFormat("text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand("text @P0 @P1", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0"),
                    new SQLiteParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
        }

        public static IEnumerable<TestCaseData> NonQueryStatementFormatIfCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text {0}", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(true, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text {0}", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatIf(false, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> NonQueryStatementFormatUnlessCases()
        {
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text"),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text", parameters: null),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlNonQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text {0}", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(false, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlNonQueryCommand("text @P0 @P1", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text"),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text", parameters: null),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text {0}", new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
            yield return new TestCaseData(
                Sql.NonQueryStatementFormatUnless(true, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlNonQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> QueryStatementCases()
        {
            yield return new TestCaseData(
                Sql.QueryStatement("text"),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatement("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatement("text", new { }),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatement("text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatement("text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                    new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                }, CommandType.Text));
        }

        public static IEnumerable<TestCaseData> QueryStatementIfCases()
        {
            yield return new TestCaseData(
                Sql.QueryStatementIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementIf(true, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementIf(true, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementIf(true, "text",
                    new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.QueryStatementIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementIf(false, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementIf(false, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementIf(false, "text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> QueryStatementUnlessCases()
        {
            yield return new TestCaseData(
                Sql.QueryStatementUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementUnless(false, "text", new { }),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementUnless(false, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementUnless(false, "text",
                    new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter1"),
                        new SQLiteParameterValueStub().ToDbParameter("@Parameter2")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.QueryStatementUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementUnless(true, "text", new { }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementUnless(true, "text", new { Parameter = new SQLiteParameterValueStub() }),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementUnless(true, "text", new { Parameter1 = new SQLiteParameterValueStub(), Parameter2 = new SQLiteParameterValueStub() }),
                new SqlQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> QueryStatementFormatCases()
        {
            yield return new TestCaseData(
               Sql.QueryStatementFormat("text"),
               new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text", parameters: null),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text", new IDbParameterValue[0]),
                new SqlQueryCommand("text", new DbParameter[0], CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text", new SQLiteParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text {0}", new SQLiteParameterValueStub()),
                new SqlQueryCommand("text @P0", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand("text", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0"),
                    new SQLiteParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
            yield return new TestCaseData(
                Sql.QueryStatementFormat("text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand("text @P0 @P1", new[]
                {
                    new SQLiteParameterValueStub().ToDbParameter("@P0"),
                    new SQLiteParameterValueStub().ToDbParameter("@P1")
                }, CommandType.Text));
        }

        public static IEnumerable<TestCaseData> QueryStatementFormatIfCases()
        {
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text {0}", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(true, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text", new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text {0}", new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatIf(false, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
        }

        public static IEnumerable<TestCaseData> QueryStatementFormatUnlessCases()
        {
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text"),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text", parameters: null),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text", new IDbParameterValue[0]),
                new[] { new SqlQueryCommand("text", new DbParameter[0], CommandType.Text) });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text {0}", new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(false, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new[]
                {
                    new SqlQueryCommand("text @P0 @P1", new[]
                    {
                        new SQLiteParameterValueStub().ToDbParameter("@P0"),
                        new SQLiteParameterValueStub().ToDbParameter("@P1")
                    }, CommandType.Text)
                });

            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text"),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text", parameters: null),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text", new IDbParameterValue[0]),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text", new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text {0}", new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
            yield return new TestCaseData(
                Sql.QueryStatementFormatUnless(true, "text {0} {1}", new SQLiteParameterValueStub(), new SQLiteParameterValueStub()),
                new SqlQueryCommand[0]);
        }
    }
}