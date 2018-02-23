using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac
{
    /// <summary>
    /// Represents the available <see cref="ProjectionHandlerResolver{TConnection}">resolvers</see>.
    /// </summary>
    public static class Resolve
    {
        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection}">handlers</see> that match the type of the message exactly.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection> WhenEqualToHandlerMessageType<TConnection>(ProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null) 
                throw new ArgumentNullException(nameof(handlers));
            var cache = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
            return message =>
            {
                if(message == null)
                    throw new ArgumentNullException(nameof(message));
                ProjectionHandler<TConnection>[] result;
                return cache.TryGetValue(message.GetType(), out result) ? 
                    result :
                    new ProjectionHandler<TConnection>[0];
            };
        }

        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection}">handlers</see> to which the message instance is assignable.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection> WhenAssignableToHandlerMessageType<TConnection>(ProjectionHandler<TConnection>[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));
            var cache = new Dictionary<Type, ProjectionHandler<TConnection>[]>();
            return message =>
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));
                ProjectionHandler<TConnection>[] result;
                if (!cache.TryGetValue(message.GetType(), out result))
                {
                    result = Array.FindAll(handlers, 
                        handler => handler.Message.IsInstanceOfType(message));
                    cache.Add(message.GetType(), result);
                }
                return result;
            };
        }

        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection, TMetadata}">handlers</see> that match the type of the message exactly.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection, TMetadata}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection, TMetadata> WhenEqualToHandlerMessageType<TConnection, TMetadata>(ProjectionHandler<TConnection, TMetadata>[] handlers)
        {
            if (handlers == null) 
                throw new ArgumentNullException(nameof(handlers));
            var cache = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
            return message =>
            {
                if(message == null)
                    throw new ArgumentNullException(nameof(message));
                ProjectionHandler<TConnection, TMetadata>[] result;
                return cache.TryGetValue(message.GetType(), out result) ? 
                    result :
                    new ProjectionHandler<TConnection, TMetadata>[0];
            };
        }

        /// <summary>
        /// Resolves the <see cref="ProjectionHandler{TConnection, TMetadata}">handlers</see> to which the message instance is assignable.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="ProjectionHandlerResolver{TConnection, TMetadata}">resolver</see>.</returns>
        public static ProjectionHandlerResolver<TConnection, TMetadata> WhenAssignableToHandlerMessageType<TConnection, TMetadata>(ProjectionHandler<TConnection, TMetadata>[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException(nameof(handlers));
            var cache = new Dictionary<Type, ProjectionHandler<TConnection, TMetadata>[]>();
            return message =>
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));
                ProjectionHandler<TConnection, TMetadata>[] result;
                if (!cache.TryGetValue(message.GetType(), out result))
                {
                    result = Array.FindAll(handlers, 
                        handler => handler.Message.IsInstanceOfType(message));
                    cache.Add(message.GetType(), result);
                }
                return result;
            };
        }
    }
}
