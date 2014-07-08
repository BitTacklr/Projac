#if FSHARP
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

namespace Projac
{
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue BigInt(FSharpOption<long> value)
        {
            if (FSharpOption<long>.get_IsNone(value))
                return TSqlBigIntNullValue.Instance;
            return new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue BigInt(long value)
        {
            return new TSqlBigIntValue(value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Int(FSharpOption<int> value)
        {
            if (FSharpOption<int>.get_IsNone(value))
                return TSqlIntNullValue.Instance;
            return new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Int(int value)
        {
            return new TSqlIntValue(value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Bit(FSharpOption<bool> value)
        {
            if (FSharpOption<bool>.get_IsNone(value))
                return TSqlBitNullValue.Instance;
            return new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue Bit(bool value)
        {
            return new TSqlBitValue(value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue DateTimeOffset(FSharpOption<DateTimeOffset> value)
        {
            if (FSharpOption<DateTimeOffset>.get_IsNone(value))
                return TSqlDateTimeOffsetNullValue.Instance;
            return new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue DateTimeOffset(DateTimeOffset value)
        {
            return new TSqlDateTimeOffsetValue(value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue UniqueIdentifier(FSharpOption<Guid> value)
        {
            if (FSharpOption<Guid>.get_IsNone(value))
                return TSqlUniqueIdentifierNullValue.Instance;
            return new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="ITSqlParameterValue" />.</returns>
        public static ITSqlParameterValue UniqueIdentifier(Guid value)
        {
            return new TSqlUniqueIdentifierValue(value);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlQueryStatement Query(string text, FSharpList<Tuple<string, ITSqlParameterValue>> parameters = null)
        {
            return new TSqlQueryStatement(text, CollectFromFSharpList(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQuery(string text, FSharpList<Tuple<string, ITSqlParameterValue>> parameters = null)
        {
            return new TSqlNonQueryStatement(text, CollectFromFSharpList(parameters));
        }

        private static SqlParameter[] CollectFromFSharpList(FSharpList<Tuple<string, ITSqlParameterValue>> parameters)
        {
            if (parameters == null)
                return new SqlParameter[0];
            return parameters.
                Select(_ => _.Item2.ToSqlParameter(FormatSqlParameterName(_.Item1))).
                ToArray();
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlQueryStatement Query(string text, FSharpMap<string, ITSqlParameterValue> parameters = null)
        {
            return new TSqlQueryStatement(text, CollectFromFSharpMap(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQuery(string text, FSharpMap<string, ITSqlParameterValue> parameters = null)
        {
            return new TSqlNonQueryStatement(text, CollectFromFSharpMap(parameters));
        }

        private static SqlParameter[] CollectFromFSharpMap(FSharpMap<string, ITSqlParameterValue> parameters)
        {
            if (parameters == null)
                return new SqlParameter[0];
            return parameters.
                Select(_ => _.Value.ToSqlParameter(FormatSqlParameterName(_.Key))).
                ToArray();
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlQueryStatement Query(string text, IDictionary<string, ITSqlParameterValue> parameters = null)
        {
            return new TSqlQueryStatement(text, CollectFromDictionary(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="TSqlNonQueryStatement" />.</returns>
        public static TSqlNonQueryStatement NonQuery(string text, IDictionary<string, ITSqlParameterValue> parameters = null)
        {
            return new TSqlNonQueryStatement(text, CollectFromDictionary(parameters));
        }

        private static SqlParameter[] CollectFromDictionary(IDictionary<string, ITSqlParameterValue> parameters)
        {
            if (parameters == null)
                return new SqlParameter[0];
            return parameters.
                Select(_ => _.Value.ToSqlParameter(FormatSqlParameterName(_.Key))).
                ToArray();
        }
    }
}
#endif