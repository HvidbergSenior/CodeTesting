namespace deftq.BuildingBlocks.Application.Commands
{
    public class EmptyCommandResponse : ICommandResponse
    {
        public static EmptyCommandResponse Default { get { return new EmptyCommandResponse(); } }
    }
}
