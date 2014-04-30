using System;

namespace Projac.Testing
{
    /// <summary>
    /// Represent a projection test specification.
    /// </summary>
    public class TSqlProjectionTestSpecification
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;
        private readonly ITSqlProjectionExpectation[] _expectations;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlProjectionTestSpecification"/> class.
        /// </summary>
        /// <param name="projection">The projection under test.</param>
        /// <param name="givens">The givens.</param>
        /// <param name="when">The when.</param>
        /// <param name="expectations">The expectations.</param>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="projection"/> or <paramref name="givens"/> or <paramref name="when"/> or <paramref name="expectations"/> is <c>null</c>.</exception>
        public TSqlProjectionTestSpecification(TSqlProjection projection, object[] givens, object when, ITSqlProjectionExpectation[] expectations)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            if (givens == null) throw new ArgumentNullException("givens");
            if (when == null) throw new ArgumentNullException("when");
            if (expectations == null) throw new ArgumentNullException("expectations");
            _projection = projection;
            _givens = givens;
            _when = when;
            _expectations = expectations;
        }

        /// <summary>
        /// Gets the projection under test.
        /// </summary>
        /// <value>
        /// The projection.
        /// </value>
        public TSqlProjection Projection
        {
            get { return _projection; }
        }

        /// <summary>
        /// Gets the givens.
        /// </summary>
        /// <value>
        /// The givens.
        /// </value>
        public object[] Givens
        {
            get { return _givens; }
        }

        /// <summary>
        /// Gets the when.
        /// </summary>
        /// <value>
        /// The when.
        /// </value>
        public object When
        {
            get { return _when; }
        }

        /// <summary>
        /// Gets the expectations.
        /// </summary>
        /// <value>
        /// The expectations.
        /// </value>
        public ITSqlProjectionExpectation[] Expectations
        {
            get { return _expectations; }
        }

        //public TSqlProjectionTestResult Fail(ITSqlProjectionExpectation failures)
        //{
            
        //}

        //public TSqlProjectionTestResult Pass()
        //{
            
        //}
    }
}