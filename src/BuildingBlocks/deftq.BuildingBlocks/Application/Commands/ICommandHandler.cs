using MediatR;

namespace deftq.BuildingBlocks.Application.Commands
{
    public interface ICommandHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
    }
}
