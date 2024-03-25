namespace deftq.BuildingBlocks.Application.IntegrationEvents
{
    public class IntegrationEventBroker : IIntegrationEventBroker
    {
        public Task Send<TCommand>(TCommand command, CancellationToken cancellationToken) where TCommand : IIntegrationEvent
        {
            //throw new NotSupportedException();
            return Task.CompletedTask;
        }
    }
}
