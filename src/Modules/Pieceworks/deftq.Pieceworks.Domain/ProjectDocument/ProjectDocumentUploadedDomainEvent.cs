using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.projectDocument
{
    public sealed class ProjectDocumentUploadedDomainEvent : DomainEvent
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId? ProjectFolderId { get; private set; }
        public ProjectDocumentId ProjectDocumentId { get; private set; }
        public ProjectDocumentName ProjectDocumentName { get; private set; }
        public ProjectDocumentUploadedTimestamp UploadedTimestamp { get; set; }

        private ProjectDocumentUploadedDomainEvent(ProjectId projectId, ProjectFolderId? projectFolderId,
            ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName,
            ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
            ProjectDocumentName = projectDocumentName;
            UploadedTimestamp = uploadedTimestamp;
        }

        public static ProjectDocumentUploadedDomainEvent Create(ProjectId projectId, ProjectFolderId? projectFolderId,
            ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName,
            ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            return new ProjectDocumentUploadedDomainEvent(projectId, projectFolderId, projectDocumentId,
                projectDocumentName, uploadedTimestamp);
        }
    }
}
