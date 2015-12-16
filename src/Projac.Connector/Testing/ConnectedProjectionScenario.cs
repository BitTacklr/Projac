using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projac.Connector.Testing
{
    /// <summary>
    /// Represent a scenario that tests a set of <see cref="ConnectedProjectionHandler{TConnection}"/>s.
    /// </summary>
    /// <typeparam name="TConnection">The type of the connection.</typeparam>
    public class ConnectedProjectionScenario<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _messages;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedProjectionScenario{TConnection}"/> class.
        /// </summary>
        /// <param name="resolver">The projection handler resolver.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="resolver"/> is <c>null</c>.</exception>
        public ConnectedProjectionScenario(ConnectedProjectionHandlerResolver<TConnection> resolver)
        {
            if (resolver == null) 
                throw new ArgumentNullException("resolver");
            _resolver = resolver;
            _messages = new object[0];
        }

        private ConnectedProjectionScenario(ConnectedProjectionHandlerResolver<TConnection> resolver, object[] messages)
        {
            _resolver = resolver;
            _messages = messages;
        }

        /// <summary>
        /// Given the following specified messages to project.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>A new <see cref="ConnectedProjectionScenario{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> is <c>null</c>.</exception>
        public ConnectedProjectionScenario<TConnection> Given(params object[] messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            return new ConnectedProjectionScenario<TConnection>(
                _resolver,
                _messages.Concat(messages).ToArray());
        }

        /// <summary>
        /// Given the following specified messages to project.
        /// </summary>
        /// <param name="messages">The messages to project.</param>
        /// <returns>A new <see cref="ConnectedProjectionScenario{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="messages"/> is <c>null</c>.</exception>
        public ConnectedProjectionScenario<TConnection> Given(IEnumerable<object> messages)
        {
            return new ConnectedProjectionScenario<TConnection>(
                _resolver,
                _messages.Concat(messages).ToArray());
            
        }

        /// <summary>
        /// Builds a test specification using the specified verification method.
        /// </summary>
        /// <param name="verification">The verification method.</param>
        /// <returns>A <see cref="ConnectedProjectionTestSpecification{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="verification"/> is <c>null</c>.</exception>
        public ConnectedProjectionTestSpecification<TConnection> Verify(Func<TConnection, Task<VerificationResult>> verification)
        {
            if (verification == null) 
                throw new ArgumentNullException("verification");
            return new ConnectedProjectionTestSpecification<TConnection>(
                _resolver,
                _messages,
                (connection, token) => verification(connection));
        }

        /// <summary>
        /// Builds a test specification using the specified verification method that takes a cancellation token.
        /// </summary>
        /// <param name="verification">The verification method.</param>
        /// <returns>A <see cref="ConnectedProjectionTestSpecification{TConnection}"/>.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="verification"/> is <c>null</c>.</exception>
        public ConnectedProjectionTestSpecification<TConnection> Verify(Func<TConnection, CancellationToken, Task<VerificationResult>> verification)
        {
            if (verification == null) 
                throw new ArgumentNullException("verification");
            return new ConnectedProjectionTestSpecification<TConnection>(
                _resolver,
                _messages,
                verification);
        }
    }
}