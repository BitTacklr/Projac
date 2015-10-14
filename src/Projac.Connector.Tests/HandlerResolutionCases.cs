using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Projac.Connector.Tests
{
    public class HandlerResolutionCases
    {
        public static IEnumerable<TestCaseData> WhenEqualToHandlerMessageTypeCases()
        {
            // * Matches *

            //Exact message type resolution
            var handler1 = HandlerFor<Message1>();
            yield return new TestCaseData(
                new[] { handler1 },
                new Message1(),
                new[] { handler1 }).SetDescription("Exact message type resolution");
            //Exact message envelope type resolution
            var handler2 = HandlerFor<MessageEnvelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler2 },
                new MessageEnvelope<Message1>(),
                new[] { handler2 }).SetDescription("Exact message envelope type resolution");
            //Partial match resolution
            var handler3 = HandlerFor<Message1>();
            var handler4 = HandlerFor<Message2>();
            yield return new TestCaseData(
                new[] { handler3, handler4 },
                new Message2(),
                new[] { handler4 }).SetDescription("Partial match resolution");

            // * Mismatches *

            //Envelope with derived type resolution
            var handler10 = HandlerFor<Envelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler10 },
                new MessageEnvelope<Message1>(),
                new ConnectedProjectionHandler<object>[0]).SetDescription("Envelope with derived type resolution");

            //No match resolution
            var handler11 = HandlerFor<OtherMessage>();
            yield return new TestCaseData(
                new[] { handler11 },
                new Message1(),
                new ConnectedProjectionHandler<object>[0]).SetDescription("No match resolution");

            // * Handler order */

            //Handler order is preserved resolution
            var handler20 = HandlerFor<Message1>();
            var handler21 = HandlerFor<Message1>();
            yield return new TestCaseData(
                new[] { handler20, handler21 },
                new Message1(),
                new[] { handler20, handler21 }).SetDescription("Handler order is preserved resolution");
            yield return new TestCaseData(
                new[] { handler21, handler20 },
                new Message1(),
                new[] { handler21, handler20 }).SetDescription("Handler order is preserved resolution");
        }

        public static IEnumerable<TestCaseData> WhenAssignableToHandlerMessageTypeCases()
        {
            // * Matches *

            //Exact message type resolution
            var handler1 = HandlerFor<Message1>();
            yield return new TestCaseData(
                new[] { handler1 },
                new Message1(),
                new[] { handler1 }).SetDescription("Exact message type resolution");
            //Message interface type resolution
            var handler2 = HandlerFor<IMessage>();
            yield return new TestCaseData(
                new[] { handler2 },
                new Message1(),
                new[] { handler2 }).SetDescription("Message interface type resolution");
            //Exact envelope and message type resolution
            var handler3 = HandlerFor<MessageEnvelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler3 },
                new MessageEnvelope<Message1>(),
                new[] { handler3 }).SetDescription("Exact envelope and message type resolution");
            //Envelope interface and exact message type resolution
            var handler4 = HandlerFor<Envelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler4 },
                new MessageEnvelope<Message1>(),
                new[] { handler4 }).SetDescription("Envelope interface and exact message type resolution");
            //Envelope and message interface type resolution
            var handler5 = HandlerFor<Envelope<IMessage>>();
            yield return new TestCaseData(
                new[] { handler5 },
                new MessageEnvelope<Message1>(),
                new[] { handler5 }).SetDescription("Envelope and message interface type resolution");
            //Mismatch ignored resolution
            var handler6 = HandlerFor<Envelope<OtherMessage>>();
            var handler7 = HandlerFor<Envelope<IMessage>>();
            yield return new TestCaseData(
                new[] { handler6, handler7 },
                new MessageEnvelope<Message1>(),
                new[] { handler7 }).SetDescription("Mismatch ignored resolution");
            //Multimatch resolution
            var handler8 = HandlerFor<Envelope<IMessage>>();
            var handler9 = HandlerFor<Envelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler8, handler9 },
                new MessageEnvelope<Message1>(),
                new[] { handler8, handler9 }).SetDescription("Multimatch resolution");
            //Derived multimatch resolution
            var handler10 = HandlerFor<Envelope<IMessage>>();
            var handler11 = HandlerFor<Envelope<Message1>>();
            yield return new TestCaseData(
                new[] { handler10, handler11 },
                new MessageEnvelope<Message2>(),
                new[] { handler10, handler11 }).SetDescription("Derived multimatch resolution");
            //Value type resolution
            var handler12 = HandlerFor<Int32>();
            yield return new TestCaseData(
                new[] { handler12 },
                42,
                new[] { handler12 }).SetDescription("Value type resolution");
            //Reference type resolution
            var handler13 = HandlerFor<String>();
            yield return new TestCaseData(
                new[] { handler13 },
                "42",
                new[] { handler13 }).SetDescription("Reference type resolution");
            //Envelope with value type resolution
            var handler14 = HandlerFor<Envelope<Int32>>();
            yield return new TestCaseData(
                new[] { handler14 },
                new MessageEnvelope<Int32>(),
                new[] { handler14 }).SetDescription("Envelope with value type resolution");
            //Envelope with reference type resolution
            var handler16 = HandlerFor<Envelope<String>>();
            yield return new TestCaseData(
                new[] { handler16 },
                new MessageEnvelope<String>(),
                new[] { handler16 }).SetDescription("Envelope with reference type resolution");
            //Envelope with reference type's interface resolution
            var handler17 = HandlerFor<Envelope<IConvertible>>();
            yield return new TestCaseData(
                new[] { handler17 },
                new MessageEnvelope<String>(),
                new[] { handler17 }).SetDescription("Envelope with reference type's interface resolution");
            //Envelope with contravariant message type resolution
            var handler18 = HandlerFor<Envelope<IEnumerable<object>>>();
            yield return new TestCaseData(
                new[] { handler18 },
                new MessageEnvelope<IEnumerable<object>>(),
                new[] { handler18 }).SetDescription("Envelope with contravariant message type resolution");
            //Envelope with contravariant derived message type resolution
            var handler19 = HandlerFor<Envelope<IEnumerable<IMessage>>>();
            yield return new TestCaseData(
                new[] { handler19 },
                new MessageEnvelope<IEnumerable<Message1>>(),
                new[] { handler19 }).SetDescription("Envelope with contravariant derived message type resolution");
            //Envelope base type resolution
            var handler20 = HandlerFor<Envelope>();
            yield return new TestCaseData(
                new[] { handler20 },
                new MessageEnvelope<Message2>(),
                new[] { handler20 }).SetDescription("Envelope base type resolution");
            //Envelope mismatch ignored resolution
            var handler21 = HandlerFor<MessageEnvelope<Message1>>();
            var handler22 = HandlerFor<MessageEnvelope<OtherMessage>>();
            yield return new TestCaseData(
                new[] { handler21, handler22 },
                new MessageEnvelope<Message1>(),
                new[] { handler21 }).SetDescription("Envelope mismatch ignored resolution");

            // * Mismatches *

            //Envelope with value type's interface resolution
            var handler15 = HandlerFor<Envelope<IConvertible>>();
            yield return new TestCaseData(
                new[] { handler15 },
                new MessageEnvelope<Int32>(),
                new ConnectedProjectionHandler<object>[0]).SetDescription("Envelope with value type's interface resolution");

            //Envelope and message interface type resolution
            var handler30 = HandlerFor<Envelope<OtherMessage>>();
            yield return new TestCaseData(
                new[] { handler30 },
                new MessageEnvelope<Message1>(),
                new ConnectedProjectionHandler<object>[0]).SetDescription("Envelope and message interface type resolution");

            //Concrete envelope with message base type resolution
            var handler31 = HandlerFor<MessageEnvelope<IMessage>>();
            yield return new TestCaseData(
                new[] { handler31 },
                new MessageEnvelope<Message1>(),
                new ConnectedProjectionHandler<object>[0]).SetDescription("Concrete envelope with message base type resolution");

            // * Handler order */

            //Handler order is preserved resolution
            var handler40 = HandlerFor<Envelope<Message1>>();
            var handler41 = HandlerFor<Envelope<IMessage>>();
            yield return new TestCaseData(
                new[] { handler40, handler41 },
                new MessageEnvelope<Message1>(),
                new[] { handler40, handler41 }).SetDescription("Handler order is preserved resolution");
            yield return new TestCaseData(
                new[] { handler41, handler40 },
                new MessageEnvelope<Message1>(),
                new[] { handler41, handler40 }).SetDescription("Handler order is preserved resolution");

        }

        private static ConnectedProjectionHandler<object> HandlerFor<TMessage>()
        {
            return new ConnectedProjectionHandler<object>(typeof(TMessage), (_, __, ___) => Task.FromResult<object>(null));
        }

        private interface IMessage { }
        private class Message1 : IMessage { }
        private class Message2 : Message1 { }
        private class OtherMessage { }
        private interface Envelope { }
        private interface Envelope<out TMessage> : Envelope { TMessage Message { get; } }
        private class MessageEnvelope<TMessage> : Envelope<TMessage>
        {
            public TMessage Message
            {
                get { return default(TMessage); }
            }
        }
    }
}
