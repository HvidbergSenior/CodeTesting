using deftq.BuildingBlocks.Integration;
using deftq.BuildingBlocks.Integration.Outbox;
using Xunit;

namespace deftq.BuildingBlocks.Test.Integration
{
    public class OutboxMessageTests
    {
        private sealed class TestIntegrationEvent : IntegrationEvent
        {
            public TestIntegrationEvent(Guid id, DateTime occurredOn) : base(id, occurredOn)
            {
            }
        }

        [Fact]
        public void Will_Create_OutboxMessage_From_IntegrationEvent_With_Same_Id()
        {
            var @event = new TestIntegrationEvent(Guid.NewGuid(), DateTime.Now);
            var message = OutboxMessage.From(@event);
            Assert.NotNull(message);
            Assert.Equal(@event.Id, message.Id);
        }

        [Fact]
        public void MyTestMethod()
        {
            var @event = new TestIntegrationEvent(Guid.NewGuid(), DateTime.Now);
            var message = OutboxMessage.From(@event);
            var serializer = new OutboxMessageSerializer();

            var eventFromJson = serializer.Deserialize(message.Payload, message.MessageType)!;

            Assert.NotNull(eventFromJson);
        }
    }
}
