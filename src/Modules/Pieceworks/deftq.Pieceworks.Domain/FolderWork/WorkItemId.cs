using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemId : ValueObject
    {
        public Guid Value { get; private set; }

        private WorkItemId()
        {
            Value = Guid.Empty;
        }

        private WorkItemId(Guid value)
        {
            Value = value;
        }

        public static WorkItemId Create(Guid value)
        {
            return new WorkItemId(value);
        }

        public static WorkItemId Empty()
        {
            return new WorkItemId(Guid.Empty);
        }
    }
}
