using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
#if !FSHARP
using System;

namespace Projac
{
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue BigInt(long? value)
        {
            if(!value.HasValue)
                return TSqlBigIntNullValue.Instance;
            return new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Int(int? value)
        {
            if(!value.HasValue)
                return TSqlIntNullValue.Instance;
            return new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Bit(bool? value)
        {
            if(!value.HasValue)
                return TSqlBitNullValue.Instance;
            return new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue UniqueIdentifier(Guid? value)
        {
            if(!value.HasValue)
                return TSqlUniqueIdentifierNullValue.Instance;
            return new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue DateTimeOffset(DateTimeOffset? value)
        {
            if(!value.HasValue)
                return TSqlDateTimeOffsetNullValue.Instance;
            return new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlQueryStatement Query(string text, object parameters = null)
        {
            return new TSqlQueryStatement(text, CollectFromAnonymousType(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQuery(string text, object parameters = null)
        {
            return new TSqlNonQueryStatement(text, CollectFromAnonymousType(parameters));
        }

        private static SqlParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new SqlParameter[0];
            return parameters.
                GetType().
                GetProperties(BindingFlags.Instance | BindingFlags.Public).
                Where(property => typeof(ITSqlParameterValue).IsAssignableFrom(property.PropertyType)).
                Select(property =>
                    ((ITSqlParameterValue)property.GetGetMethod().Invoke(parameters, null)).
                        ToSqlParameter(FormatSqlParameterName(property.Name))).
                ToArray();
        }
    }
}
#endif