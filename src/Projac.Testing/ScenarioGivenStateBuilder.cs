using System;
using System.Collections.Generic;
using System.Linq;

namespace Projac.Testing
{
    internal class ScenarioGivenStateBuilder : IScenarioGivenStateBuilder
    {
        private readonly TSqlProjection _projection;
        private readonly object[] _givens;

        public ScenarioGivenStateBuilder(TSqlProjection projection, object[] givens)
        {
            _projection = projection;
            _givens = givens;
        }

        public IScenarioGivenStateBuilder Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new ScenarioGivenStateBuilder(_projection, _givens.Concat(events).ToArray());
        }

        public IScenarioGivenStateBuilder Given(IEnumerable<object> events)
        {
            if (events == null) throw new ArgumentNullException("events");
            return new ScenarioGivenStateBuilder(_projection, _givens.Concat(events).ToArray());
        }

        public IScenarioWhenStateBuilder When(object @event)
        {
            if (@event == null) throw new ArgumentNullException("event");
            return new ScenarioWhenStateBuilder(_projection, _givens, @event);
        }
    }
}