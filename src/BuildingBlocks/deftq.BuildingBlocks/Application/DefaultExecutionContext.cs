namespace deftq.BuildingBlocks.Application
{
    public class DefaultExecutionContext : IExecutionContext
    {
        public Guid UserId { get => Guid.Empty; }

        public string UserName => "default";
    }
}