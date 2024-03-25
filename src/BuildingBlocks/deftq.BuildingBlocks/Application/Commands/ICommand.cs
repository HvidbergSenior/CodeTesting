using MediatR;

namespace deftq.BuildingBlocks.Application.Commands
{
    public interface ICommand<out T> : IRequest<T>
    {
    }

    public interface ICommand : IRequest
    { }
}
