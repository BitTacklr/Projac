using System;

namespace Projac.Testing
{
    public class TSqlProjectionTestSpecification
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;
        private readonly object _when;
        private readonly ITSqlProjectionVerification[] _verifications;

        public TSqlProjectionTestSpecification(TSqlProjection projection, object[] givens, object when, ITSqlProjectionVerification[] verifications)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            if (givens == null) throw new ArgumentNullException("givens");
            if (when == null) throw new ArgumentNullException("when");
            if (verifications == null) throw new ArgumentNullException("verifications");
            _projection = projection;
            _givens = givens;
            _when = when;
            _verifications = verifications;
        }

        public TSqlProjection Projection
        {
            get { return _projection; }
        }

        public object[] Givens
        {
            get { return _givens; }
        }

        public object When
        {
            get { return _when; }
        }

        public ITSqlProjectionVerification[] Verifications
        {
            get { return _verifications; }
        }
    }
}