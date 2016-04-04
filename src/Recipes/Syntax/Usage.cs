using Paramol.SqlClient;
using Projac;
using Recipes.Shared;

namespace Recipes.Syntax
{
    public class Usage
    {
        public class SampleUsingProjection : SqlProjection<SqlClientSyntax>
        {
            public SampleUsingProjection()
            {
                When<PortfolioAdded>(_ =>
                    Sql.NonQueryStatement(
                        "INSERT INTO [Portfolio] () VALUES ()"));

                When<PortfolioRenamed>(_ =>
                    Sql.NonQueryStatement(
                        "UPDATE [Portfolio] SET WHERE"));

                When<PortfolioRemoved>(_ =>
                    Sql.NonQueryStatement(
                        "DELETE FROM [Portfolio] WHERE [Id]=@Id", Sql.UniqueIdentifier(_.Id)));
            }
        }

        /// <summary>
        ///     Represent a SQL projection with syntax.
        /// </summary>
        public abstract class SqlProjection<TSyntax> : SqlProjection
            where TSyntax : new()
        {
            private static readonly TSyntax Syntax = new TSyntax();

            /// <summary>
            ///     Gets the sql syntax associated with this projection.
            /// </summary>
            /// <value>
            ///     The sql syntax associated with this projection.
            /// </value>
            protected TSyntax Sql
            {
                get { return Syntax; }
            }
        }
    }
}
