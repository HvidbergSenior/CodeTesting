using System.Diagnostics.CodeAnalysis;

namespace deftq.BuildingBlocks.Integration.Outbox
{
    public sealed class OutboxMessage
    {
        public Guid Id { get; private set; }

        public DateTimeOffset OccurredOn { get; private set; }

        public string MessageType { get; private set; } = default!;

        public string Payload { get; private set; } = default!;

        public DateTime? ProcessedDate { get; private set; }

        private OutboxMessage(Guid id, DateTimeOffset occurredOn, string messageType, string payload)
        {
            this.Id = id;
            this.OccurredOn = occurredOn;
            this.MessageType = messageType;
            this.Payload = payload;
        }

        public static OutboxMessage From([NotNull] IntegrationEvent @event)
        {
            var messageType = @event.GetType().AssemblyQualifiedName;
            var @eventType = Type.GetType(messageType!);

            var serializer = new OutboxMessageSerializer();
            var payload = serializer.Serialize(@event, eventType);

            return new OutboxMessage(@event.Id, @event.OccurredOn, messageType!, payload);
        }

        internal void Processed()
        {
            ProcessedDate = DateTime.UtcNow;
        }

        private OutboxMessage()
        {
        }
    }
}
