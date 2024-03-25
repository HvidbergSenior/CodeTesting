using MediatR;

namespace deftq.BuildingBlocks.Application.Queries
{
    public interface IQuery<out T> : IRequest<T>
    {
    }
}
