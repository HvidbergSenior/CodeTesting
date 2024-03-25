using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemMovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId SourceProjectFolderId { get; }
        public ProjectFolderId DestinationProjectFolderId { get; }
        public IList<WorkItemId> WorkItemIds { get; }
        private WorkItemMovedDomainEvent(ProjectId projectId, ProjectFolderId sourceProjectFolderId, ProjectFolderId destinationProjectFolderId, IList<WorkItemId> workItemIds)
        {
            ProjectId = projectId;
            SourceProjectFolderId = sourceProjectFolderId;
            DestinationProjectFolderId = destinationProjectFolderId;
            WorkItemIds = workItemIds;
        }

        public static WorkItemMovedDomainEvent Create(ProjectId projectId, ProjectFolderId sourceProjectFolderId, ProjectFolderId destinationProjectFolderId, IList<WorkItemId> workItemIds)
        {
            return new WorkItemMovedDomainEvent(projectId, sourceProjectFolderId, destinationProjectFolderId, workItemIds);
        }
    }
}
