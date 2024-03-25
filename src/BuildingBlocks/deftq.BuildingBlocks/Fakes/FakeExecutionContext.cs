using deftq.BuildingBlocks.Application;

namespace deftq.BuildingBlocks.Fakes
{
    public class FakeExecutionContext : IExecutionContext
    {
        public Guid UserId { get; }
        
        public string UserName { get; }
        
        public FakeExecutionContext() : this(Guid.NewGuid())
        {
        }

        public FakeExecutionContext(Guid userId, string userName = "test")
        {
            UserId = userId;
            UserName = userName;
        }
    }
}