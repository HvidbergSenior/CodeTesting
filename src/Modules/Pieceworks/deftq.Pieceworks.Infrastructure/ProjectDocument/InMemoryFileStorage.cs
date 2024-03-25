using Baseline;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public class InMemoryFileStorage : IFileStorage
    {
        private Dictionary<InMemoryFileReference, byte[]> files = new();

        public InMemoryFileStorage()
        {
            
        }

        public Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<IFileReference>>(files
                .Where(f => f.Key.ProjectId == projectId && f.Key.ProjectFolderId is null)
                .Select(f => f.Key).ToList());
        }

        public Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<IFileReference>>(
                files.Where(f => f.Key.ProjectId == projectId)
                    .Where(f => f.Key.ProjectFolderId is not null && f.Key.ProjectFolderId == projectFolderId)
                    .Select(f => f.Key).ToList());
        }

        public Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectDocumentId id, ProjectDocumentName name,
            Stream content, CancellationToken cancellationToken)
        {
            var bytes = content.ReadAllBytes();
            var reference = InMemoryFileReference.Create(projectId, null, id);
            files[reference] = bytes;
            return Task.FromResult((IFileReference)reference);
        }

        public Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            var bytes = content.ReadAllBytes();
            var reference = InMemoryFileReference.Create(projectId, projectFolderId, id);
            files[reference] = bytes;
            return Task.FromResult((IFileReference)reference);
        }

        public async Task<byte[]> GetFileContentAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not InMemoryFileReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }

            var inMemoryFileReference = (InMemoryFileReference)fileReference;
            try
            {
                var file = files.First(fr => fr.Key == inMemoryFileReference);
                await using (var stream = new MemoryStream(file.Value))
                {
                    return await stream.ReadAllBytesAsync();
                }
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"In memory reference ${inMemoryFileReference.ToString()} not found");
            }
        }

        public Task DeleteFileAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not InMemoryFileReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }

            var inMemoryFileReference = (InMemoryFileReference)fileReference;
            
            var file =
                files.Where(f => f.Key.ProjectId == inMemoryFileReference.ProjectId && f.Key.ProjectDocumentId == inMemoryFileReference.ProjectDocumentId)
                    .Select(f => f.Key).FirstOrDefault();
            if (file is not null)
            {
                files.Remove(file);
            }
            return Task.CompletedTask;
        }

        public Task DeleteFilesAsync(ProjectId projectId, CancellationToken cancellationToken)
        {
            var filesFromProject = files.Where(f => f.Key.ProjectId == projectId).Select(f => f.Key);
            foreach (var file in filesFromProject)
            {
                files.Remove(file);
            }

            return Task.CompletedTask;
        }

        public Task DeleteFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            CancellationToken cancellationToken)
        {
            var filesFromFolder = files.Where(f =>
                f.Key.ProjectId == projectId && f.Key.ProjectFolderId is not null &&
                f.Key.ProjectFolderId == projectFolderId).Select(f => f.Key);
            foreach (var file in filesFromFolder)
            {
                files.Remove(file);
            }

            return Task.CompletedTask;
        }
    }
}
