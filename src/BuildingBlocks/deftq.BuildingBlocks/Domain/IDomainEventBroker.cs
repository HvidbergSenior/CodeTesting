namespace deftq.BuildingBlocks.Domain
{
    public interface IDomainEventBroker
    {
        Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : DomainEvent;
    }
}
