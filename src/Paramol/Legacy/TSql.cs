using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    [Obsolete("Please use the SqlClientSyntax instead. This will be removed in a future release.")]
    public static partial class TSql
    {
        private static DbParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return ThrowIfMaxParameterCountExceeded(
                parameters.
                    GetType().
                    GetProperties(BindingFlags.Instance | BindingFlags.Public).
                    Where(property => typeof(IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                    Select(property =>
                        ((IDbParameterValue)property.GetGetMethod().Invoke(parameters, null)).
                            ToDbParameter(FormatDbParameterName(property.Name))).
                    ToArray());
        }

        private static DbParameter[] ThrowIfMaxParameterCountExceeded(DbParameter[] parameters)
        {
            if (parameters.Length > Limits.MaxParameterCount)
                throw new ArgumentException(
                    string.Format("The parameter count is limited to {0}.", Limits.MaxParameterCount),
                    "parameters");
            return parameters;
        }

        // ReSharper disable UnusedParameter.Local
        private static void ThrowIfMaxParameterCountExceeded(IDbParameterValue[] parameters)
        // ReSharper restore UnusedParameter.Local
        {
            if (parameters.Length > Limits.MaxParameterCount)
                throw new ArgumentException(
                    string.Format("The parameter count is limited to {0}.", Limits.MaxParameterCount),
                    "parameters");
        }

        private static string FormatDbParameterName(string name)
        {
            return "@" + name;
        }
    }
}