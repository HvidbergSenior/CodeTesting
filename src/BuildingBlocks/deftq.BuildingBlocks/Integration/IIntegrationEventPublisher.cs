namespace deftq.BuildingBlocks.Integration
{
    public interface IIntegrationEventPublisher
    {
        Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken);
    }
}
