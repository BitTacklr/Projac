using System;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework;

namespace Projac.Tests.Framework
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RequiresSqlServerAttribute : Attribute, ITestAction
    {
        private const string CallContextKey = "Projac.SqlServerInstanceDiscoveryResult";
        private readonly DatabaseOperations _databaseOperations;

        public RequiresSqlServerAttribute()
        {
            _databaseOperations = new DatabaseOperations();
        }

        public void BeforeTest(TestDetails testDetails)
        {
            var result = _databaseOperations.DiscoverSqlServerInstance();
            _databaseOperations.RecreateDatabase(result);
            SetDiscoveryResult(result);
        }

        public void AfterTest(TestDetails testDetails)
        {
            _databaseOperations.DetachDatabase(GetDiscoveryResult());
        }

        private static void SetDiscoveryResult(SqlServerInstanceDiscoveryResult result)
        {
            CallContext.SetData(CallContextKey, result);
        }

        private static SqlServerInstanceDiscoveryResult GetDiscoveryResult()
        {
            return (SqlServerInstanceDiscoveryResult) CallContext.GetData(CallContextKey);
        }

        public ActionTargets Targets
        {
            get { return ActionTargets.Suite; }
        }
    }
}