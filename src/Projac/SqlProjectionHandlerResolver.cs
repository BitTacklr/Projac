namespace Projac
{
    /// <summary>
    /// Resolves the <see cref="SqlProjectionHandler">handlers</see> that match the specified <paramref name="message"/>.
    /// </summary>
    /// <param name="message">The message to resolve <see cref="SqlProjectionHandler">handlers</see> for.</param>
    /// <returns>The set of matching <see cref="SqlProjectionHandler">handlers</see>.</returns>
    public delegate SqlProjectionHandler[] SqlProjectionHandlerResolver(object message);
}