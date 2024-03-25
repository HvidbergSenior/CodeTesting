namespace deftq.BuildingBlocks.Integration
{
    public abstract class IntegrationEvent : IIntegrationEvent
    {
        protected IntegrationEvent(Guid id, DateTimeOffset occurredOn)
        {
            Id = id;
            OccurredOn = occurredOn;
        }

        public Guid Id { get; protected set; }

        public DateTimeOffset OccurredOn { get; protected set; }
    }
}
