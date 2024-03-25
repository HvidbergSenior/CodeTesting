using MediatR;

namespace deftq.BuildingBlocks.Application.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IMediator mediator;

        public CommandBus(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Send<TCommand>(ICommand command, CancellationToken cancellationToken)
        {
            await mediator.Send(command, cancellationToken);
        }

        public async Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
        {
            return await mediator.Send(command, cancellationToken);
        }
    }
}
