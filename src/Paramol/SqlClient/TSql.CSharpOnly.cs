using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;

#if !FSHARP

namespace Paramol.SqlClient
{
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue BigInt(long? value)
        {
            if (!value.HasValue)
                return TSqlBigIntNullValue.Instance;
            return new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Int(int? value)
        {
            if (!value.HasValue)
                return TSqlIntNullValue.Instance;
            return new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Bit(bool? value)
        {
            if (!value.HasValue)
                return TSqlBitNullValue.Instance;
            return new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue UniqueIdentifier(Guid? value)
        {
            if (!value.HasValue)
                return TSqlUniqueIdentifierNullValue.Instance;
            return new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue DateTimeOffset(DateTimeOffset? value)
        {
            if (!value.HasValue)
                return TSqlDateTimeOffsetNullValue.Instance;
            return new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlQueryStatement Query(string text, object parameters = null)
        {
            return new SqlQueryStatement(text, CollectFromAnonymousType(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return Query(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlQueryStatement> QueryUnless(bool condition, string text, object parameters = null)
        {
            if (!condition)
                yield return Query(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static SqlNonQueryCommand NonQuery(string text, object parameters = null)
        {
            return new SqlNonQueryStatement(text, CollectFromAnonymousType(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement if the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryIf(bool condition, string text, object parameters = null)
        {
            if (condition)
                yield return NonQuery(text, parameters);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement unless the condition is satisfied.
        /// </summary>
        /// <param name="condition">The condition to satisfy</param>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryCommand" />.</returns>
        public static IEnumerable<SqlNonQueryCommand> NonQueryUnless(bool condition, string text,
            object parameters = null)
        {
            if (!condition)
                yield return NonQuery(text, parameters);
        }

        private static DbParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return parameters.
                GetType().
                GetProperties(BindingFlags.Instance | BindingFlags.Public).
                Where(property => typeof (IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                Select(property =>
                    ((IDbParameterValue) property.GetGetMethod().Invoke(parameters, null)).
                        ToDbParameter(FormatDbParameterName(property.Name))).
                ToArray();
        }
    }
}

#endif