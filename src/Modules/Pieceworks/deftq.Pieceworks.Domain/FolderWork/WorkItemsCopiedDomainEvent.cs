using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemsCopiedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId DestinationFolderId { get; }
        public IList<WorkItemId> WorkItemIds { get; }

        private WorkItemsCopiedDomainEvent(ProjectId projectId, ProjectFolderId destinationFolderId, IList<WorkItemId> workItemIds)
        {
            ProjectId = projectId;
            DestinationFolderId = destinationFolderId;
            WorkItemIds = workItemIds;
        }

        public static WorkItemsCopiedDomainEvent Create(ProjectId projectId, ProjectFolderId destinationFolderId, IList<WorkItemId> workItemIds)
        {
            return new WorkItemsCopiedDomainEvent(projectId, destinationFolderId, workItemIds);
        }
    }
}