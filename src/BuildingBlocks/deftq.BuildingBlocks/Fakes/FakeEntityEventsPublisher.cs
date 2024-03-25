
using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Events;

namespace deftq.BuildingBlocks.Fakes
{
    public sealed class FakeEntityEventsPublisher : IEntityEventsPublisher
    {
        public bool HasBeenCalled { get; private set; }

        public FakeEntityEventsPublisher()
        {
            HasBeenCalled = false;
        }

        public IReadOnlyCollection<IDomainEvent> EnqueueEventsFrom(Entity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return entity.DomainEvents.ToArray();
        }

        public Task Publish(CancellationToken cancellationToken = default)
        {
            HasBeenCalled = true;
            return Task.CompletedTask;
        }

        public bool TryEnqueueEventsFrom(Entity entity, out IReadOnlyCollection<IDomainEvent> uncommittedEvents)
        {
            HasBeenCalled = true;
            uncommittedEvents = Array.Empty<IDomainEvent>();
            return true;
        }
    }
}
