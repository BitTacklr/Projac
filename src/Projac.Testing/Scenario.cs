using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    /// <summary>
    /// The given-when-expect test specification builder bootstrapper.
    /// </summary>
    public class Scenario : IScenarioInitialStateBuilder
    {
        private readonly TSqlProjection _projection;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenario"/> class.
        /// </summary>
        /// <param name="projection">The projection under test.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="projection"/> is <c>null</c>.</exception>
        public Scenario(TSqlProjection projection)
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
        public IScenarioGivenStateBuilder Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new ScenarioGivenStateBuilder(_projection, events);
        }

        /// <summary>
        /// Given the following events occured.
        /// </summary>
        /// <param name="events">The events that occurred.</param>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="events"/> is <c>null</c>.</exception>
        public IScenarioGivenStateBuilder Given(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new ScenarioGivenStateBuilder(_projection, events.ToArray());
        }

        /// <summary>
        /// Given no events occured.
        /// </summary>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        public IScenarioGivenNoneStateBuilder GivenNone()
        {
            return new ScenarioGivenNoneStateBuilder(_projection);
        }

        /// <summary>
        /// When an event occurs.
        /// </summary>
        /// <param name="event">The event that occurred.</param>
        /// <returns>
        /// A builder continuation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Throw when <paramref name="event"/> is <c>null</c>.</exception>
        public IScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new ScenarioWhenStateBuilder(_projection, new object[0], @event);
        }
    }
}
