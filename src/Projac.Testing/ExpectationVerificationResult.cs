using System.IO;

namespace Projac.Testing
{
    /// <summary>
    /// Represents an expectation verification result.
    /// </summary>
    public abstract class ExpectationVerificationResult
    {
        private readonly IExpectation _expectation;
        private readonly ExpectationVerificationResultState _state;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectationVerificationResult"/> class.
        /// </summary>
        /// <param name="expectation">The verified expectation.</param>
        /// <param name="state">The verification result state.</param>
        protected ExpectationVerificationResult(IExpectation expectation, ExpectationVerificationResultState state)
        {
            _expectation = expectation;
            _state = state;
        }

        /// <summary>
        /// Gets the expectation associated with this result.
        /// </summary>
        /// <value>
        /// The expectation.
        /// </value>
        public IExpectation Expectation { get { return _expectation; } }
        /// <summary>
        /// Gets a value indicating whether this <see cref="ExpectationVerificationResult"/> has passed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if passed; otherwise, <c>false</c>.
        /// </value>
        public bool Passed { get { return _state == ExpectationVerificationResultState.Passed; } }
        /// <summary>
        /// Gets a value indicating whether this <see cref="ExpectationVerificationResult"/> has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if failed; otherwise, <c>false</c>.
        /// </value>
        public bool Failed { get { return _state == ExpectationVerificationResultState.Failed; } }
        /// <summary>
        /// Writes a description of itself to the specified <paramref name="writer"/>.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        public abstract void WriteTo(TextWriter writer);
    }
}