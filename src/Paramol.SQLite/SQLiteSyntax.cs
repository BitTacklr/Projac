using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Paramol.SQLite
{
    /// <summary>
    ///     Fluent SQLite syntax.
    /// </summary>
    public partial class SQLiteSyntax
    {
        private static DbParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return parameters.
                    GetType().
                    GetProperties(BindingFlags.Instance | BindingFlags.Public).
                    Where(property => typeof(IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                    Select(property =>
                        ((IDbParameterValue)property.GetGetMethod().Invoke(parameters, null)).
                            ToDbParameter(FormatDbParameterName(property.Name))).
                    ToArray();
        }

        private static string FormatDbParameterName(string name)
        {
            return "@" + name;
        }
    }
}
