namespace deftq.BuildingBlocks.Application.Commands
{
    public interface ICommandBus
    {
        Task Send<TCommand>(ICommand command, CancellationToken cancellationToken);

        Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);
    }
}
