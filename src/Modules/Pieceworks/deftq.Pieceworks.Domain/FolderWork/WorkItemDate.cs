using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemDate : ValueObject
    {
        public DateOnly Value { get; private set; }

        private WorkItemDate()
        {
            Value = DateOnly.MinValue;
        }

        private WorkItemDate(DateOnly value)
        {
            Value = value;
        }

        public static WorkItemDate Create(DateOnly value)
        {
            return new WorkItemDate(value);
        }

        public static WorkItemDate Empty()
        {
            return new WorkItemDate(DateOnly.MinValue);
        }
        
        public static WorkItemDate Today()
        {
            return new WorkItemDate(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}
