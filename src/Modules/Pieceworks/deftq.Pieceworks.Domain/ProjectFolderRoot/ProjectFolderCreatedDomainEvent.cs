using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderCreatedDomainEvent : DomainEvent
    {
        public ProjectFolderId ProjectFolderId { get; }
        
        private ProjectFolderCreatedDomainEvent(ProjectFolderId projectFolderId)
        {
            ProjectFolderId = projectFolderId;
        }

        public static ProjectFolderCreatedDomainEvent Create(ProjectFolderId projectFolderId)
        {
            return new ProjectFolderCreatedDomainEvent(projectFolderId);
        }
    }
}
