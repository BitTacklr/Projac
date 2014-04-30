using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    /// <summary>
    /// The given-when-expect test specification builder bootstrapper.
    /// </summary>
    public class TSqlProjectionScenario : ITSqlProjectionScenarioInitialStateBuilder
    {
        private readonly TSqlProjection _projection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TSqlProjectionScenario"/> class.
        /// </summary>
        /// <param name="projection">The projection under test.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public TSqlProjectionScenario(TSqlProjection projection)
        {
            if (projection == null) throw new ArgumentNullException("projection");
            _projection = projection;
        }

        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="events"/> is <c>null</c>.</exception>
        public ITSqlProjectionScenarioGivenStateBuilder Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, events);
        }

        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="events"/> is <c>null</c>.</exception>
        public ITSqlProjectionScenarioGivenStateBuilder Given(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new TSqlProjectionScenarioGivenStateBuilder(_projection, events.ToArray());
        }

        /// <summary>
        /// Given no events occured.
        /// </summary>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        public ITSqlProjectionScenarioGivenNoneStateBuilder GivenNone()
        {
            return new TSqlProjectionScenarioGivenNoneStateBuilder(_projection);
        }

        /// <summary>
        /// When an event occurs.
        /// </summary>
        /// <param name="event">The event that occurred.</param>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="event"/> is <c>null</c>.</exception>
        public ITSqlProjectionScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new TSqlProjectionScenarioWhenStateBuilder(_projection, new object[0], @event);
        }
    }
}
