namespace deftq.BuildingBlocks.Application.IntegrationEvents
{
    public interface IIntegrationEventBroker
    {
        Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : IIntegrationEvent;
    }
}
