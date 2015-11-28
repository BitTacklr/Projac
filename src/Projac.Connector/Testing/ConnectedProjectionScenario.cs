using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projac.Connector.Testing
{
    public class ConnectedProjectionScenario<TConnection> : IConnectedProjectionScenarioInitialState<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;

        public ConnectedProjectionScenario(ConnectedProjectionHandlerResolver<TConnection> resolver)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            _resolver = resolver;
        }

        public IConnectedProjectionScenarioGivenState<TConnection> Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            return new ConnectedProjectionScenarioGivenState<TConnection>(_resolver, events);
        }
    }


    internal class ConnectedProjectionScenarioInitialState<TConnection> : IConnectedProjectionScenarioInitialState<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _givens;

        public ConnectedProjectionScenarioInitialState(ConnectedProjectionHandlerResolver<TConnection> resolver, object[] givens)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (givens == null) throw new ArgumentNullException(nameof(givens));
            _resolver = resolver;
            _givens = givens;
        }

        public IConnectedProjectionScenarioGivenState<TConnection> Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            var givens = new object[_givens.Length + events.Length];
            Array.Copy(_givens, givens, _givens.Length);
            Array.Copy(events, 0, givens, _givens.Length, events.Length);
            return new ConnectedProjectionScenarioGivenState<TConnection>(_resolver, givens);
        }
    }

    internal class ConnectedProjectionScenarioGivenState<TConnection> : IConnectedProjectionScenarioGivenState<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _givens;

        public ConnectedProjectionScenarioGivenState(ConnectedProjectionHandlerResolver<TConnection> resolver, object[] givens)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (givens == null) throw new ArgumentNullException(nameof(givens));
            _resolver = resolver;
            _givens = givens;
        }

        public IConnectedProjectionScenarioGivenState<TConnection> Given(params object[] events)
        {
            if (events == null) throw new ArgumentNullException(nameof(events));
            var givens = new object[_givens.Length + events.Length];
            Array.Copy(_givens, givens, _givens.Length);
            Array.Copy(events, 0, givens, _givens.Length, events.Length);
            return new ConnectedProjectionScenarioGivenState<TConnection>(_resolver, givens);
        }

        public IConnectedProjectionScenarioExpectState<TConnection> Expect(ConnectedProjectionExpectation<TConnection> expectation)
        {
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));
            throw new NotImplementedException();
        }
    }

    internal class ConnectedProjectionScenarioExpectState<TConnection> : IConnectedProjectionScenarioExpectState<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _givens;
        private readonly ConnectedProjectionExpectation<TConnection> _expectation;

        public ConnectedProjectionScenarioExpectState(ConnectedProjectionHandlerResolver<TConnection> resolver, object[] givens, ConnectedProjectionExpectation<TConnection> expectation)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (givens == null) throw new ArgumentNullException(nameof(givens));
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));
            _resolver = resolver;
            _givens = givens;
            _expectation = expectation;
        }

        public ConnectedProjectionTestSpecification<TConnection> Build()
        {
            return new ConnectedProjectionTestSpecification<TConnection>(_resolver, _givens, _expectation);
        }
    }

    public interface IConnectedProjectionScenarioInitialState<TConnection>
    {
        IConnectedProjectionScenarioGivenState<TConnection> Given(params object[] @events);
    }

    public interface IConnectedProjectionScenarioGivenState<TConnection>
    {
        IConnectedProjectionScenarioGivenState<TConnection> Given(params object[] @events);

        IConnectedProjectionScenarioExpectState<TConnection> Expect(ConnectedProjectionExpectation<TConnection> expectation);
    }

    public interface IConnectedProjectionScenarioExpectState<TConnection>
    {
        ConnectedProjectionTestSpecification<TConnection> Build();
    }

    public class ConnectedProjectionTestSpecification<TConnection>
    {
        private readonly ConnectedProjectionHandlerResolver<TConnection> _resolver;
        private readonly object[] _givens;
        private readonly ConnectedProjectionExpectation<TConnection> _expectation;

        public ConnectedProjectionTestSpecification(ConnectedProjectionHandlerResolver<TConnection> resolver, object[] givens, ConnectedProjectionExpectation<TConnection> expectation)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (givens == null) throw new ArgumentNullException(nameof(givens));
            if (expectation == null) throw new ArgumentNullException(nameof(expectation));
            _resolver = resolver;
            _givens = givens;
            _expectation = expectation;
        }

        public ConnectedProjectionHandlerResolver<TConnection> Resolver
        {
            get { return _resolver; }
        }

        public object[] Givens
        {
            get { return _givens; }
        }

        public ConnectedProjectionExpectation<TConnection> Expectation
        {
            get { return _expectation; }
        }

        public async Task<ExpectationResult> Verify(TConnection connection)
        {
            var projector = new ConnectedProjector<TConnection>(_resolver);
            await projector.ProjectAsync(connection, _givens);
            return await _expectation(connection);
        }
    }

    public delegate Task<ExpectationResult> ConnectedProjectionExpectation<TConnection>(TConnection connection);

    public class ExpectationResult
    {
    }
}

