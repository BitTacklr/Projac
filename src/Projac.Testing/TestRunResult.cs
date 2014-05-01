using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    /// <summary>
    /// Represents the result of a running a test specification.
    /// </summary>
    public class TestRunResult {
        private readonly TestSpecification _specification;
        private readonly ExpectationVerificationResult[] _verificationResults;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRunResult"/> class.
        /// </summary>
        /// <param name="specification">The specification.</param>
        /// <param name="verificationResults">The results.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="specification"/> or <paramref name="verificationResults"/> is <c>null</c>.</exception>
        public TestRunResult(TestSpecification specification, ExpectationVerificationResult[] verificationResults)
        {
            if (specification == null) throw new ArgumentNullException("specification");
            if (verificationResults == null) throw new ArgumentNullException("verificationResults");
            _specification = specification;
            _verificationResults = verificationResults;
        }

        /// <summary>
        /// Gets the test specification associated with this result.
        /// </summary>
        /// <value>
        /// The test specification.
        /// </value>
        public TestSpecification Specification
        {
            get { return _specification; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TestRunResult"/> has passed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if passed; otherwise, <c>false</c>.
        /// </value>
        public bool Passed
        {
            get { return VerificationResults.All(_ => _.Passed); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="TestRunResult"/> has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed
        {
            get { return VerificationResults.Any(_ => _.Failed); }
        }

        /// <summary>
        /// Gets the expectation verification results.
        /// </summary>
        /// <value>
        /// The expectation verification results.
        /// </value>
        public IReadOnlyCollection<ExpectationVerificationResult> VerificationResults
        {
            get { return _verificationResults; }
        }
    }
}