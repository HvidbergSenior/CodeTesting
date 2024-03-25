using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public sealed class InMemoryFileReference : ValueObject, IFileReference
    {
        public ProjectId ProjectId { get; private set; }
        public ProjectFolderId? ProjectFolderId { get; private set; }
        public ProjectDocumentId ProjectDocumentId { get; private set; }

        public InMemoryFileReference()
        {
            ProjectId = ProjectId.Empty();
            ProjectFolderId = ProjectFolderId.Empty();
            ProjectDocumentId = ProjectDocumentId.Empty();
        }

        private InMemoryFileReference(ProjectId projectId, ProjectFolderId? projectFolderId,
            ProjectDocumentId projectDocumentId)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            ProjectDocumentId = projectDocumentId;
        }

        public override string ToString()
        {
            return $"{ProjectId.Value}.{ProjectFolderId?.Value}.{ProjectDocumentId.Value}";
        }

        internal static InMemoryFileReference Create(ProjectId projectId, ProjectFolderId? projectFolderId,
            ProjectDocumentId projectDocumentId)
        {
            return new InMemoryFileReference(projectId, projectFolderId, projectDocumentId);
        }
    }
}
