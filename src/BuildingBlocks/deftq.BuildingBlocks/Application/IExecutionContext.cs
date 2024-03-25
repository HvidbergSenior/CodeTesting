namespace deftq.BuildingBlocks.Application
{
    public interface IExecutionContext
    {
        public Guid UserId { get; }
        public string UserName { get; }
    }
}