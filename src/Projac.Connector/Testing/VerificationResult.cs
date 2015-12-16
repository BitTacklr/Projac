using System;

namespace Projac.Connector.Testing
{
    /// <summary>
    /// The result of a test specification verification.
    /// </summary>
    public struct VerificationResult : IEquatable<VerificationResult>
    {
        private readonly VerificationResultState _state;
        private readonly string _message;

        private VerificationResult(VerificationResultState state, string message)
        {
            _state = state;
            _message = message;
        }

        /// <summary>
        /// Returns <c>true</c> if the test specification verification passed.
        /// </summary>
        public bool Passed { get { return _state == VerificationResultState.Passed; } }

        /// <summary>
        /// Returns <c>false</c> if the test specification verification failed.
        /// </summary>
        public bool Failed { get { return _state == VerificationResultState.Failed; } }

        /// <summary>
        /// Returns a message stating why the test specification verification passed or failed, or an empty if it wasn't specified.
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// Indicates that the test specification verification passed.
        /// </summary>
        /// <param name="message">An optional message stating why test specification verification passed.</param>
        /// <returns>A passed <see cref="VerificationResult"/>.</returns>
        public static VerificationResult Pass(string message = "")
        {
            if (message == null) 
                throw new ArgumentNullException("message");

            return new VerificationResult(VerificationResultState.Passed, message);
        }

        /// <summary>
        /// Indicates that the test specification verification failed.
        /// </summary>
        /// <param name="message">An optional message stating why test specification verification failed.</param>
        /// <returns>A failed <see cref="VerificationResult"/>.</returns>
        public static VerificationResult Fail(string message = "")
        {
            if (message == null) 
                throw new ArgumentNullException("message");
            
            return new VerificationResult(VerificationResultState.Failed, message);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(VerificationResult other)
        {
            return _state == other._state && string.Equals(_message, other._message);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is VerificationResult && Equals((VerificationResult)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _state.GetHashCode() ^ _message.GetHashCode();
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(VerificationResult left, VerificationResult right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(VerificationResult left, VerificationResult right)
        {
            return !left.Equals(right);
        }
    }
}