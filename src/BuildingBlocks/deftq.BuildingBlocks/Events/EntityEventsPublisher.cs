using deftq.BuildingBlocks.Domain;
using MediatR;

namespace deftq.BuildingBlocks.Events
{
    public class EntityEventsPublisher : IEntityEventsPublisher
    {
        private readonly IMediator mediator;

        private readonly Queue<IDomainEvent> pendingEvents = new();
        public EntityEventsPublisher(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public IReadOnlyCollection<IDomainEvent> EnqueueEventsFrom(Entity entity)
        {
            IReadOnlyCollection<IDomainEvent> uncomittedEvents = new List<IDomainEvent>(entity.DomainEvents).AsReadOnly();
            entity.ClearDomainEvents();
            foreach (var item in uncomittedEvents)
            {
                pendingEvents.Enqueue(item);
            }

            return uncomittedEvents;
        }

        public async Task Publish(CancellationToken cancellationToken = default)
        {
            var eventsToPublish = pendingEvents.ToArray();
            pendingEvents.Clear();
            foreach (var evt in eventsToPublish)
            {
                await mediator.Publish(evt, cancellationToken);
            }
        }

        public bool TryEnqueueEventsFrom(Entity entity, out IReadOnlyCollection<IDomainEvent> uncomittedEvents)
        {
            uncomittedEvents = EnqueueEventsFrom(entity);
            return true;
        }
    }
}
