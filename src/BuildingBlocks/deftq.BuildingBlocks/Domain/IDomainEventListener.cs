using MediatR;

namespace deftq.BuildingBlocks.Domain
{
    public interface IDomainEventListener<in TRequest> : INotificationHandler<TRequest> where TRequest : IDomainEvent
    {
    }
}
