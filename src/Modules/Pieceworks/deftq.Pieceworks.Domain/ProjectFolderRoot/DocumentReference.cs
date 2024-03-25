using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.projectDocument;

namespace deftq.Pieceworks.Domain.projectFolderRoot
{
    public sealed class DocumentReference : ValueObject
    {
        public ProjectDocumentId ProjectDocumentId { get; private set; }
        public ProjectDocumentName ProjectDocumentName { get; private set; }
        public ProjectDocumentUploadedTimestamp UploadedTimestamp  { get; private set; }

        private DocumentReference()
        {
            ProjectDocumentId = ProjectDocumentId.Empty();
            ProjectDocumentName = ProjectDocumentName.Empty();
            UploadedTimestamp = ProjectDocumentUploadedTimestamp.Empty();
        } 
        
        private DocumentReference(ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName, ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            ProjectDocumentId = projectDocumentId;
            ProjectDocumentName = projectDocumentName;
            UploadedTimestamp = uploadedTimestamp;
        }

        public static DocumentReference Create(ProjectDocumentId projectDocumentId, ProjectDocumentName projectDocumentName, ProjectDocumentUploadedTimestamp uploadedTimestamp)
        {
            return new DocumentReference(projectDocumentId, projectDocumentName, uploadedTimestamp);
        }
    }
}
