using System.Collections.Generic;

namespace Projac.Testing
{
    /// <summary>
    /// The initial state within the test specification building process.
    /// </summary>
    public interface ITSqlProjectionScenarioInitialStateBuilder
    {
        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events);

        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events);

        /// <summary>
        /// Given no events occured.
        /// </summary>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioGivenNoneStateBuilder GivenNone();

        /// <summary>
        /// When an event occurs.
        /// </summary>
        /// <param name="event">The event that occurred.</param>
        /// <returns>A builder continuation.</returns>
        ITSqlProjectionScenarioWhenStateBuilder When(object @event);
    }
}