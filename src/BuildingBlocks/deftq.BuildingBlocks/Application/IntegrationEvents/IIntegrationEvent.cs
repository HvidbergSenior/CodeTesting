using MediatR;

namespace deftq.BuildingBlocks.Application.IntegrationEvents
{
    public interface IIntegrationEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}
