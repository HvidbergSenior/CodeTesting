using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class ProjectDocumentRemovedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; }
        public ProjectFolderId ProjectFolderId { get; }
        public ProjectDocumentId ProjectDocumentId { get; }

        private ProjectDocumentRemovedDomainEvent(ProjectId projectId, ProjectFolderId projectFolderId, ProjectDocumentId projectDocumentId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
        }

        public static ProjectDocumentRemovedDomainEvent Create(ProjectId projectId, ProjectFolderId projectFolderId, ProjectDocumentId projectDocumentId)
        {
            return new ProjectDocumentRemovedDomainEvent(projectId, projectFolderId, projectDocumentId);
        }
    }
}
