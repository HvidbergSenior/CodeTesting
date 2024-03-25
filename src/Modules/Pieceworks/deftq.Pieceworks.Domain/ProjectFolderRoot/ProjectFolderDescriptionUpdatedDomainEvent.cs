using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectFolderDescriptionUpdatedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectFolderDescription ProjectFolderDescription { get; }
        
        private ProjectFolderDescriptionUpdatedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderDescription projectFolderDescription)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectFolderDescription = projectFolderDescription;
        }

        public static ProjectFolderDescriptionUpdatedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, ProjectFolderDescription projectFolderDescription)
        {
            return new ProjectFolderDescriptionUpdatedDomainEvent(projectId, projectFolderId, projectFolderDescription);
        }
    }
}
