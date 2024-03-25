using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderNameUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectFolderName ProjectFolderName { get; }

        private ProjectFolderNameUpdatedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderName projectFolderName)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectFolderName = projectFolderName;
        }

        public static ProjectFolderNameUpdatedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderName projectFolderName)
        {
            return new ProjectFolderNameUpdatedDomainEvent(projectId, projectFolderId, projectFolderName);
        }
    }
}
