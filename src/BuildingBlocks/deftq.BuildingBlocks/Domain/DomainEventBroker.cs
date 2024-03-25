namespace deftq.BuildingBlocks.Domain
{
    public class DomainEventBroker : IDomainEventBroker
    {
        public Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : DomainEvent
        {
            throw new NotSupportedException();
        }
    }
}
