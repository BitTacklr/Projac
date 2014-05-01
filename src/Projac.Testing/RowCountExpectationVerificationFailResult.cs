using System.IO;

namespace Projac.Testing
{
    class RowCountExpectationVerificationFailResult : ExpectationVerificationResult
    {
        private readonly int _actualRowCount;

        public RowCountExpectationVerificationFailResult(IExpectation expectation, int actualRowCount)
            : base(expectation, ExpectationVerificationResultState.Failed)
        {
            _actualRowCount = actualRowCount;
        }

        public override void WriteTo(TextWriter writer)
        {
            throw new System.NotImplementedException();
        }
    }
}