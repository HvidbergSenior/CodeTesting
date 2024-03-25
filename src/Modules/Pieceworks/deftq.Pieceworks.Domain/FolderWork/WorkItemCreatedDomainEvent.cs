using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemCreatedDomainEvent : DomainEvent
    {
        public WorkItemId WorkItemId { get; private set; }

        private WorkItemCreatedDomainEvent(WorkItemId workItemId)
        {
            WorkItemId = workItemId;
        }

        public static WorkItemCreatedDomainEvent Create(WorkItemId workItemId)
        {
            return new WorkItemCreatedDomainEvent(workItemId);
        }
    }
}