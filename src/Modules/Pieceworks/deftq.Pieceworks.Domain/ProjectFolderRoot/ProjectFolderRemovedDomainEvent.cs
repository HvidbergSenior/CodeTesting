using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderRemovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        
        private ProjectFolderRemovedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
        }

        public static ProjectFolderRemovedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId)
        {
            return new ProjectFolderRemovedDomainEvent(projectId, projectFolderId);
        }
    }
}
