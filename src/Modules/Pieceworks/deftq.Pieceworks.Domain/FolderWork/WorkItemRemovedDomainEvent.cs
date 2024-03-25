using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemRemovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public IList<WorkItemId> WorkItemId { get; }
        
        public WorkItemRemovedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, IList<WorkItemId> workItemId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemId = workItemId;
        }

        public static WorkItemRemovedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, IList<WorkItemId> workItemId)
        {
            return new WorkItemRemovedDomainEvent(projectId, projectFolderId, workItemId);
        }
    }
}
