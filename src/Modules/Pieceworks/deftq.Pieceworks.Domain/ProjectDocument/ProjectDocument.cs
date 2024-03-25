using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.projectDocument
{
    public sealed class ProjectDocument : Entity
    {
        public ProjectId ProjectId { get; private set; }
        
        public ProjectFolderId? ProjectFolderId { get; private set; }
        
        public ProjectDocumentId ProjectDocumentId { get; private set; }
        
        public ProjectDocumentName ProjectDocumentName { get; private set; }
        
        public ProjectDocumentUploadedTimestamp UploadedTimestamp { get; private set; }
        
        public IFileReference FileReference { get; private set; }

        private ProjectDocument()
        {
            Id = Guid.NewGuid();
            ProjectId = ProjectId.Create(Id);
            ProjectFolderId = null;
            ProjectDocumentId = ProjectDocumentId.Empty();
            ProjectDocumentName = ProjectDocumentName.Empty();
            UploadedTimestamp = ProjectDocumentUploadedTimestamp.Empty();
            FileReference = IFileReference.Empty();
        }

        private ProjectDocument(ProjectId projectId, ProjectFolderId? projectFolderId, ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName, ProjectDocumentUploadedTimestamp uploadedTimestamp, IFileReference fileReference)
        {
            Id = projectDocumentId.Value;
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
            ProjectDocumentName = projectDocumentName;
            UploadedTimestamp = uploadedTimestamp;
            FileReference = fileReference;
        }

        public static ProjectDocument Create(ProjectId projectId, ProjectFolderId? projectFolderId, ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName, ProjectDocumentUploadedTimestamp uploadedTimestamp, IFileReference fileReference)
        {
            var projectDocument = new ProjectDocument(projectId, projectFolderId, projectDocumentId, projectDocumentName, uploadedTimestamp, fileReference);
            projectDocument.AddDomainEvent(ProjectDocumentUploadedDomainEvent.Create(projectId, projectFolderId, projectDocumentId, projectDocumentName, uploadedTimestamp));
            return projectDocument;
        }
    }
}
