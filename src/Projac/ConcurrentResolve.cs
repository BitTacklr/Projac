using System;
using System.Collections.Concurrent;

namespace Projac
{
    /// <summary>
    /// Represents the available concurrent <see cref="ProjectionHandlerResolver{TConnection}">resolvers</see>.
    /// </summary>
    public static class ConcurrentResolve
    {
        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection}">handlers</see> that match the type of the message exactly.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection> WhenEqualToHandlerMessageType<TConnection>(ProjectionHandler<TConnection>[] handlers)
        {
            return Resolve.WhenEqualToHandlerMessageType<TConnection>(handlers);
        }

        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection}">handlers</see> to which the message instance is assignable.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection> WhenAssignableToHandlerMessageType<TConnection>(ProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");
            var cache = new ConcurrentDictionary<Type, ProjectionHandler<TConnection>[]>();
            return message =>
            {
                if (message == null)
                    throw new ArgumentNullException("message");
                ProjectionHandler<TConnection>[] result;
                if (!cache.TryGetValue(message.GetType(), out result))
                {
                    result = cache.GetOrAdd(message.GetType(), 
                        Array.FindAll(handlers, 
                            handler => handler.Message.IsInstanceOfType(message)));
                }
                return result;
            };
        }
    }
}