#if FSHARP
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Microsoft.FSharp.Core;
using Microsoft.FSharp.Collections;

namespace Paramol.SqlClient
{
    public static partial class TSql
    {
        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue BigInt(FSharpOption<long> value)
        {
            if (FSharpOption<long>.get_IsNone(value))
                return TSqlBigIntNullValue.Instance;
            return new TSqlBigIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIGINT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue BigInt(long value)
        {
            return new TSqlBigIntValue(value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Int(FSharpOption<int> value)
        {
            if (FSharpOption<int>.get_IsNone(value))
                return TSqlIntNullValue.Instance;
            return new TSqlIntValue(value.Value);
        }

        /// <summary>
        ///     Returns a INT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Int(int value)
        {
            return new TSqlIntValue(value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Bit(FSharpOption<bool> value)
        {
            if (FSharpOption<bool>.get_IsNone(value))
                return TSqlBitNullValue.Instance;
            return new TSqlBitValue(value.Value);
        }

        /// <summary>
        ///     Returns a BIT parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue Bit(bool value)
        {
            return new TSqlBitValue(value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue DateTimeOffset(FSharpOption<DateTimeOffset> value)
        {
            if (FSharpOption<DateTimeOffset>.get_IsNone(value))
                return TSqlDateTimeOffsetNullValue.Instance;
            return new TSqlDateTimeOffsetValue(value.Value);
        }

        /// <summary>
        ///     Returns a DATETIMEOFFSET parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue DateTimeOffset(DateTimeOffset value)
        {
            return new TSqlDateTimeOffsetValue(value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue UniqueIdentifier(FSharpOption<Guid> value)
        {
            if (FSharpOption<Guid>.get_IsNone(value))
                return TSqlUniqueIdentifierNullValue.Instance;
            return new TSqlUniqueIdentifierValue(value.Value);
        }

        /// <summary>
        ///     Returns a UNIQUEIDENTIFIER parameter value.
        /// </summary>
        /// <param name="value">The parameter value.</param>
        /// <returns>A <see cref="IDbParameterValue" />.</returns>
        public static IDbParameterValue UniqueIdentifier(Guid value)
        {
            return new TSqlUniqueIdentifierValue(value);
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlQueryStatement Query(string text, FSharpList<Tuple<string, IDbParameterValue>> parameters = null)
        {
            return new SqlQueryStatement(text, CollectFromFSharpList(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlNonQueryStatement NonQuery(string text, FSharpList<Tuple<string, IDbParameterValue>> parameters = null)
        {
            return new SqlNonQueryStatement(text, CollectFromFSharpList(parameters));
        }

        private static DbParameter[] CollectFromFSharpList(FSharpList<Tuple<string, IDbParameterValue>> parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return parameters.
                Select(_ => _.Item2.ToDbParameter(FormatDbParameterName(_.Item1))).
                ToArray();
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlQueryStatement Query(string text, FSharpMap<string, IDbParameterValue> parameters = null)
        {
            return new SqlQueryStatement(text, CollectFromFSharpMap(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlNonQueryStatement NonQuery(string text, FSharpMap<string, IDbParameterValue> parameters = null)
        {
            return new SqlNonQueryStatement(text, CollectFromFSharpMap(parameters));
        }

        private static DbParameter[] CollectFromFSharpMap(FSharpMap<string, IDbParameterValue> parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return parameters.
                Select(_ => _.Value.ToDbParameter(FormatDbParameterName(_.Key))).
                ToArray();
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlQueryStatement Query(string text, IDictionary<string, IDbParameterValue> parameters = null)
        {
            return new SqlQueryStatement(text, CollectFromDictionary(parameters));
        }

        /// <summary>
        ///     Returns a T-SQL non query statement.
        /// </summary>
        /// <param name="text">The text with named parameters.</param>
        /// <param name="parameters">The named parameters.</param>
        /// <returns>A <see cref="SqlNonQueryStatement" />.</returns>
        public static SqlNonQueryStatement NonQuery(string text, IDictionary<string, IDbParameterValue> parameters = null)
        {
            return new SqlNonQueryStatement(text, CollectFromDictionary(parameters));
        }

        private static DbParameter[] CollectFromDictionary(IDictionary<string, IDbParameterValue> parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
            return parameters.
                Select(_ => _.Value.ToDbParameter(FormatDbParameterName(_.Key))).
                ToArray();
        }
    }
}
#endif