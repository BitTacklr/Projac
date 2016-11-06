using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Testing
{
    /// <summary>
    /// Represent a scenario that tests a set of <see cref="ProjectionHandler{TConnection}"/>s.
    /// </summary>
    /// <typeparam name="TConnection">The type of the connection.</typeparam>
    public class ProjectionScenario<TConnection>
    {
        private readonly ProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionScenario{TConnection}"/> class.
        /// </summary>
        /// <param name="resolver">The projection handler resolver.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="resolver"/> is <c>null</c>.</exception>
        public ProjectionScenario(ProjectionHandlerResolver<TConnection> resolver)
        {
            if (resolver == null) 
                throw new ArgumentNullException("resolver");
            _resolver = resolver;
            _messages = new object[0];
        }

        private ProjectionScenario(ProjectionHandlerResolver<TConnection> resolver, object[] messages)
        {
            _resolver = resolver;
            _messages = messages;
        }

        /// <summary>
        /// Given the following specified messages to project.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>A new <see cref="ProjectionScenario{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> is <c>null</c>.</exception>
        public ProjectionScenario<TConnection> Given(params object[] messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            return new ProjectionScenario<TConnection>(
                _resolver,
                _messages.Concat(messages).ToArray());
        }

        /// <summary>
        /// Given the following specified messages to project.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>A new <see cref="ProjectionScenario{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> is <c>null</c>.</exception>
        public ProjectionScenario<TConnection> Given(IEnumerable<object> messages)
        {
            return new ProjectionScenario<TConnection>(
                _resolver,
                _messages.Concat(messages).ToArray());
            
        }

        /// <summary>
        /// Builds a test specification using the specified verification method.
        /// </summary>
        /// <param name="verification">The verification method.</param>
        /// <returns>A <see cref="ProjectionTestSpecification{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="verification"/> is <c>null</c>.</exception>
        public ProjectionTestSpecification<TConnection> Verify(Func<TConnection, Task<VerificationResult>> verification)
        {
            if (verification == null) 
                throw new ArgumentNullException("verification");
            return new ProjectionTestSpecification<TConnection>(
                _resolver,
                _messages,
                (connection, token) => verification(connection));
        }

        /// <summary>
        /// Builds a test specification using the specified verification method that takes a cancellation token.
        /// </summary>
        /// <param name="verification">The verification method.</param>
        /// <returns>A <see cref="ProjectionTestSpecification{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="verification"/> is <c>null</c>.</exception>
        public ProjectionTestSpecification<TConnection> Verify(Func<TConnection, CancellationToken, Task<VerificationResult>> verification)
        {
            if (verification == null) 
                throw new ArgumentNullException("verification");
            return new ProjectionTestSpecification<TConnection>(
                _resolver,
                _messages,
                verification);
        }
    }
}