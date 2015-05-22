namespace Projac.Connector
{
    /// <summary>
    /// Resolves the <see cref="ConnectedProjectionHandler{TConnection}">handlers</see> that match the specified <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to resolve <see cref="ConnectedProjectionHandler{TConnection}">handlers</see> for.</param>
    /// <returns>The set of matching <see cref="ConnectedProjectionHandler{TConnection}">handlers</see>.</returns>
    public delegate ConnectedProjectionHandler<TConnection>[] ConnectedProjectionHandlerResolver<TConnection>(object message);
}