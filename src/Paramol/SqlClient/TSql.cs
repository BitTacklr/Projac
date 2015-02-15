//#define REFLECTION_BASED_PARAMETER_COLLECTION
#define CACHED_REFLECTION_BASED_PARAMETER_COLLECTION
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Paramol.SqlClient
{
    /// <summary>
    ///     Fluent T-SQL syntax.
    /// </summary>
    public static partial class TSql
    {
        private static readonly CollectorCache Collectors = new CollectorCache();

        private static DbParameter[] CollectFromAnonymousType(object parameters)
        {
            if (parameters == null)
                return new DbParameter[0];
#if REFLECTION_BASED_PARAMETER_COLLECTION
            return ThrowIfMaxParameterCountExceeded(
                parameters.
                    GetType().
                    GetProperties(BindingFlags.Instance | BindingFlags.Public).
                    Where(property => typeof(IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                    Select(property =>
                        ((IDbParameterValue)property.GetGetMethod().Invoke(parameters, null)).
                            ToDbParameter(FormatDbParameterName(property.Name))).
                    ToArray());
#elif CACHED_REFLECTION_BASED_PARAMETER_COLLECTION
            return ThrowIfMaxParameterCountExceeded(
                Collectors.
                    GetOrAdd(
                        parameters.GetType(),
                        new Lazy<Func<object, DbParameter[]>>(() => CreateCollectorUsingReflection(parameters),
                            LazyThreadSafetyMode.None))
                    (parameters));
#else
            return ThrowIfMaxParameterCountExceeded(
                Collectors.
                    GetOrAdd(
                        parameters.GetType(),
                        new Lazy<Func<object, DbParameter[]>>(() => CreateCollectorUsingExpressions(parameters),
                            LazyThreadSafetyMode.None))
                    (parameters));
#endif
        }

        class CollectorCache
        {
            private Node _head;

            public CollectorCache()
            {
                _head = null;
            }

            public Func<object, DbParameter[]> GetOrAdd(Type key, Lazy<Func<object, DbParameter[]>> factory)
            {
                Func<object, DbParameter[]> collector;
                do
                {
                    var oldHead = Interlocked.CompareExchange(ref _head, null, null);
                    collector = GetCollectorOrNull(oldHead, key);
                    if (collector == null)
                    {
                        var newHead = new Node(key, factory.Value, oldHead);
                        if (Interlocked.CompareExchange(ref _head, newHead, oldHead) == oldHead)
                        {
                            collector = factory.Value;
                        }
                    }
                } while (collector == null);
                return collector;
            }

            static Func<object, DbParameter[]> GetCollectorOrNull(Node node, Type key)
            {
                while (node != null)
                {
                    if (key == node.Key)
                    {
                        return node.Collector;
                    }
                    node = node.Next;
                }
                return null;
            }

            class Node
            {
                public readonly Type Key;
                public readonly Func<object, DbParameter[]> Collector;
                public readonly Node Next;

                public Node(Type key, Func<object, DbParameter[]> collector, Node next)
                {
                    Key = key;
                    Collector = collector;
                    Next = next;
                }
            }
        }

        private static readonly MethodInfo ToDbParameterMethod =
            typeof(IDbParameterValue).GetMethods().Single();

        private static readonly MethodInfo FormatDbParameterNameMethod = typeof(TSql).GetMethod(
            "FormatDbParameterName", BindingFlags.Static | BindingFlags.NonPublic);

        private static readonly DbParameter[] EmptyDbParameterArray = new DbParameter[0];
        private static readonly Func<object, DbParameter[]> EmptyCollector = _ => EmptyDbParameterArray;

        private static Func<object, DbParameter[]> CreateCollectorUsingExpressions(object parameters)
        {
            var parametersExpression = Expression.Parameter(typeof(object), "parameters");
            var parametersType = parameters.GetType();
            var properties = parametersType.
                GetProperties(BindingFlags.Instance | BindingFlags.Public).
                Where(property => typeof (IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                ToArray();
            if (properties.Length == 0)
            {
                return EmptyCollector;
            }
            return Expression.Lambda<Func<object, DbParameter[]>>(
                Expression.NewArrayInit(
                    typeof (DbParameter),
                    properties.
                        Select(property => Expression.Call(
                            Expression.Property(
                                Expression.Convert(parametersExpression, parametersType),
                                property),
                            ToDbParameterMethod,
                            Expression.Call(
                                FormatDbParameterNameMethod,
                                Expression.Constant(property.Name)
                                )))),
                parametersExpression).
                Compile();
        }

        private static Func<object, DbParameter[]> CreateCollectorUsingReflection(object parameters)
        {
            var properties = parameters.
                    GetType().
                    GetProperties(BindingFlags.Instance | BindingFlags.Public).
                    Where(property => typeof(IDbParameterValue).IsAssignableFrom(property.PropertyType)).
                    Select(property => new Tuple<string, MethodInfo>(property.Name, property.GetGetMethod())).
                    ToArray();
            if (properties.Length == 0)
            {
                return EmptyCollector;
            }
            return _ =>
                properties.
                    Select(property =>
                        ((IDbParameterValue) property.Item2.Invoke(_, null)).
                            ToDbParameter(FormatDbParameterName(property.Item1))).
                    ToArray();
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