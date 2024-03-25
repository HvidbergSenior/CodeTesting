using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItemAddedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public WorkItemId WorkItemId { get; }

        private WorkItemAddedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, WorkItemId workItemId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItemId = workItemId;
        }

        public static WorkItemAddedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, WorkItemId workItemId)
        {
            return new WorkItemAddedDomainEvent(projectId, projectFolderId, workItemId);
        }
    }
}
