using MediatR;

namespace deftq.BuildingBlocks.Integration
{
    public class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IPublisher publisher;

        public IntegrationEventPublisher(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        public Task Publish(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
        {
            return publisher.Publish(integrationEvent, cancellationToken);
        }
    }
}
