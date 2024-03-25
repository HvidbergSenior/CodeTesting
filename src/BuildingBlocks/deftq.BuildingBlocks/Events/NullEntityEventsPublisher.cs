using deftq.BuildingBlocks.Domain;

namespace deftq.BuildingBlocks.Events
{
    public class NullEntityEventsPublisher : IEntityEventsPublisher
    {
        public IReadOnlyCollection<IDomainEvent> EnqueueEventsFrom(Entity entity)
        {
            return new List<IDomainEvent>().AsReadOnly();
        }

        public Task Publish(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public bool TryEnqueueEventsFrom(Entity entity, out IReadOnlyCollection<IDomainEvent> uncomittedEvents)
        {
            uncomittedEvents = EnqueueEventsFrom(entity);
            return true;
        }
    }
}