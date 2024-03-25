using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemUser : ValueObject
    {
        public Guid UserId { get; private set; }
        public string UserName { get; private set; }

        private WorkItemUser()
        {
            UserId = Guid.Empty;
            UserName = String.Empty;
        }

        private WorkItemUser(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public static WorkItemUser Create(Guid userId, string userName)
        {
            return new WorkItemUser(userId, userName);
        }

        public static WorkItemUser Empty()
        {
            return new WorkItemUser(Guid.Empty, string.Empty);
        }
    }
}
