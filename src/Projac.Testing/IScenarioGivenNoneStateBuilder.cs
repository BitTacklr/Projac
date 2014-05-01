namespace Projac.Testing
{
    /// <summary>
    /// The given none state within the test specification building process.
    /// </summary>
    public interface IScenarioGivenNoneStateBuilder
    {
        /// <summary>
        /// When an event occurs.
        /// </summary>
        /// <param name="event">The event that occurred</param>
        /// <returns>A builder continuation</returns>
        IScenarioWhenStateBuilder When(object @event);
    }
}