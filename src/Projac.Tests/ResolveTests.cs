using System;
using System.Collections.Generic;
using NUnit.Framework;
using Paramol;

namespace Projac.Tests
{
    public class ResolveTests
    {
        [Test]
        public void should_handle_exact_type()
        {
            var handler = HandlerFor<Message1>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new Message1()),
                Is.EquivalentTo(new[] {handler}));
        }

        [Test]
        public void should_handle_for_interface_type()
        {
            var handler = HandlerFor<IMessage>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new Message1()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_handle_for_exact_envelope_type()
        {
            var handler = HandlerFor<MessageEnvelope<Message1>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_handle_for_envelope_interface_on_exact_type()
        {
            var handler = HandlerFor<Envelope<Message1>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_handle_for_envelope_interface_on_interface_type()
        {
            var handler = HandlerFor<Envelope<IMessage>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler }));
        }


        [Test]
        public void should_not_handle_for_envelope_interface_on_different_message_type()
        {
            var handler = HandlerFor<Envelope<OtherMessage>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.Empty);
        }

        [Test]
        public void handlers_shouldnt_freak_each_other_out()
        {
            var handler1 = HandlerFor<Envelope<OtherMessage>>();
            var handler2 = HandlerFor<Envelope<IMessage>>();
            var sut = SutFactory(handler1, handler2);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler2 }));
        }


        [Test]
        public void all_handlers_for_message_should_trigger()
        {
            var handler1 = HandlerFor<Envelope<IMessage>>();
            var handler2 = HandlerFor<Envelope<Message1>>();
            var sut = SutFactory(handler1, handler2);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler1, handler2 }));
        }

        [Test]
        public void envelopes_of_derived_messages_should_get_handled()
        {
            var handler1 = HandlerFor<Envelope<IMessage>>();
            var handler2 = HandlerFor<Envelope<Message1>>();
            var sut = SutFactory(handler1, handler2);
            Assert.That(
                sut(new MessageEnvelope<Message2>()),
                Is.EquivalentTo(new[] { handler1, handler2 }));
        }


        [Test]
        public void should_work_with_value_types()
        {
            var handler = HandlerFor<Int32>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(42),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_work_with_strings()
        {
            var handler = HandlerFor<String>();
            var sut = SutFactory(handler);
            Assert.That(
                sut("42"),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_work_with_envelope_of_value_types()
        {
            var handler = HandlerFor<Envelope<Int32>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<int>()),
                Is.EquivalentTo(new[] { handler }));
        }


        [Test]
        public void should_work_with_envelope_of_strings()
        {
            var handler = HandlerFor<Envelope<String>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<String>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_work_with_enumerables_of_strings()
        {
            var handler = HandlerFor<Envelope<IEnumerable<string>>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<IEnumerable<string>>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_work_with_enumerables_of_objects()
        {
            var handler = HandlerFor<Envelope<IEnumerable<IMessage>>>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<IEnumerable<Message1>>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_maintain_order_with_message_first()
        {
            var handler1 = HandlerFor<Envelope<Message1>>();
            var handler2 = HandlerFor<Envelope<IMessage>>();
            var sut = SutFactory(handler1, handler2);
            Assert.That(
                sut(new MessageEnvelope<Message2>()),
                Is.EquivalentTo(new[] { handler1, handler2 }));
        }

        [Test]
        public void should_maintain_order_with_message_interface_first()
        {
            var handler1 = HandlerFor<Envelope<Message1>>();
            var handler2 = HandlerFor<Envelope<IMessage>>();
            var sut = SutFactory(handler2, handler1);
            Assert.That(
                sut(new MessageEnvelope<Message2>()),
                Is.EquivalentTo(new[] { handler2, handler1 }));
        }

        [Test]
        public void should_handle_for_untyped_envelope_interface()
        {
            var handler = HandlerFor<Envelope>();
            var sut = SutFactory(handler);
            Assert.That(
                sut(new MessageEnvelope<Message2>()),
                Is.EquivalentTo(new[] { handler }));
        }

        [Test]
        public void should_handle_for_generic_concrete_envelop()
        {
            var handler1 = HandlerFor<MessageEnvelope<Message1>>();
            var handler2 = HandlerFor<MessageEnvelope<OtherMessage>>();
            var sut = SutFactory(handler1, handler2);
            Assert.That(
                sut(new MessageEnvelope<Message1>()),
                Is.EquivalentTo(new[] { handler1 }));
        }

        private static SqlProjectionHandler HandlerFor<TMessage>()
        {
            return new SqlProjectionHandler(typeof(TMessage), _ => new SqlNonQueryCommand[0]);
        }

        private static SqlProjectionHandlerResolver SutFactory(params SqlProjectionHandler[] handlers)
        {
            return Resolve.WhenAssignableToType(handlers);
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
