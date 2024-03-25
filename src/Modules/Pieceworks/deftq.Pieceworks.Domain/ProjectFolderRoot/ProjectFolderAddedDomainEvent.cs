using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderAddedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        
        private ProjectFolderAddedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static ProjectFolderAddedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new ProjectFolderAddedDomainEvent(projectId, projectFolderId);
        }
    }
}
