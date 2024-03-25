using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderSupplementsUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }

        private ProjectFolderSupplementsUpdatedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static ProjectFolderSupplementsUpdatedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new ProjectFolderSupplementsUpdatedDomainEvent(projectId, projectFolderId);
        }
    }
}
