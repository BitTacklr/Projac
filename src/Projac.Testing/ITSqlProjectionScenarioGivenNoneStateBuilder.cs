namespace Projac.Testing
{
    /// <summary>
    /// The given none state within the test specification building process.
    /// </summary>
    public interface ITSqlProjectionScenarioGivenNoneStateBuilder
    {
        /// <summary>
        /// When an event occurs.
        /// </summary>
        /// <param name="event">The event that occurred</param>
        /// <returns>A builder continuation</returns>
        ITSqlProjectionScenarioWhenStateBuilder When(object @event);
    }
}