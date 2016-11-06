namespace Projac
{
    /// <summary>
    /// Resolves the <see cref="ProjectionHandler{TConnection}">handlers</see> that match the specified <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to resolve <see cref="ProjectionHandler{TConnection}">handlers</see> for.</param>
    /// <returns>The set of matching <see cref="ProjectionHandler{TConnection}">handlers</see>.</returns>
    public delegate ProjectionHandler<TConnection>[] ProjectionHandlerResolver<TConnection>(object message);
}