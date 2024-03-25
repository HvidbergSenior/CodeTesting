using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Domain.projectDocument
{
    public interface IFileStorage
    {
        Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId, CancellationToken cancellationToken);
        Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId, CancellationToken cancellationToken);

        Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken);
        Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectFolderId projectFolderId, ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken);
        
        Task<byte[]> GetFileContentAsync(IFileReference fileReference, CancellationToken cancellationToken);
        
        Task DeleteFileAsync(IFileReference fileReference, CancellationToken cancellationToken);
        Task DeleteFilesAsync(ProjectId projectId, CancellationToken cancellationToken);
        Task DeleteFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId, CancellationToken cancellationToken);
    }
}
