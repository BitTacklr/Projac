using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Projac.Testing.NUnit
{
    public static class NUnitAssertionExtensions
    {
        private static readonly Lazy<string> ConfiguredConnectionString = 
            new Lazy<string>(() => ConfigurationManager.ConnectionStrings["Projac"].ConnectionString);

        public static void Assert(this ITestSpecificationBuilder builder, string connectionString = null)
        {
            var specification = builder.Build();
            var runner = new TestSpecificationRunner(connectionString ?? ConfiguredConnectionString.Value);
            var result = runner.Run(specification);
            if (result.Passed) return;

            foreach (var verificationResult in result.VerificationResults.Where(_ => _.Failed))
            {
                using (var writer = new StringWriter())
                {
                    verificationResult.WriteTo(writer);
                }
            }
        }
    }
}
