using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac
{
    /// <summary>
    /// Represents the available <see cref="SqlProjectionHandlerResolver">resolvers</see>.
    /// </summary>
    public static class Resolve
    {
        /// <summary>
        /// Resolves the <see cref="SqlProjectionHandler">handlers</see> that match the type of the message exactly.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="SqlProjectionHandlerResolver">resolver</see>.</returns>
        public static SqlProjectionHandlerResolver WhenEqualToHandlerMessageType(SqlProjectionHandler[] handlers)
        {
            if (handlers == null) 
                throw new ArgumentNullException("handlers");
            var cache = handlers.
                GroupBy(handler => handler.Message).
                ToDictionary(@group => @group.Key, @group => @group.ToArray());
            return message =>
            {
                if(message == null)
                    throw new ArgumentNullException("message");
                SqlProjectionHandler[] result;
                return cache.TryGetValue(message.GetType(), out result) ? 
                    result : 
                    new SqlProjectionHandler[0];
            };
        }

        /// <summary>
        /// Resolves the <see cref="SqlProjectionHandler">handlers</see> to which the message instance is assignable.
        /// </summary>
        /// <param name="handlers">The set of resolvable handlers.</param>
        /// <returns>A <see cref="SqlProjectionHandlerResolver">resolver</see>.</returns>
        public static SqlProjectionHandlerResolver WhenAssignableToHandlerMessageType(SqlProjectionHandler[] handlers)
        {
            if (handlers == null)
                throw new ArgumentNullException("handlers");
            var cache = new Dictionary<Type, SqlProjectionHandler[]>();
            return message =>
            {
                if (message == null)
                    throw new ArgumentNullException("message");
                SqlProjectionHandler[] result;
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
