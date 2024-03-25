using Baseline;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public class FilesystemFileStorage : IFileStorage
    {
        private const string TempFolderName = "akkord_plus_document_repository";
        private String _repositoryPath;

        public FilesystemFileStorage()
        {
            _repositoryPath = Path.Join(Path.GetTempPath(), TempFolderName);
            if (!Directory.Exists(_repositoryPath))
            {
                Directory.CreateDirectory(_repositoryPath);
            }
        }

        public Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId,
            CancellationToken cancellationToken)
        {
            var documents = new List<IFileReference>();
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            if (Directory.Exists(projectPath))
            {
                var files = Directory.EnumerateFiles(projectPath);
                foreach (var file in files)
                {
                    documents.Add(FilesystemFileReference.Create(Path.GetFullPath(file)));
                }
            }

            return Task.FromResult(documents.AsEnumerable());
        }

        public Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            CancellationToken cancellationToken)
        {
            var documents = new List<IFileReference>();
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            var folderPath = Path.Join(projectPath, projectFolderId.Value.ToString());
            if (Directory.Exists(folderPath))
            {
                var files = Directory.EnumerateFiles(folderPath);
                foreach (var file in files)
                {
                    documents.Add(FilesystemFileReference.Create(Path.GetFullPath(file)));
                }
            }

            return Task.FromResult(documents.AsEnumerable());
        }

        public async Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectDocumentId id,
            ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            var documentPath = Path.Join(projectPath, id.Value.ToString());
            if (!Directory.Exists(projectPath))
            {
                Directory.CreateDirectory(projectPath);
            }

            using (var file = File.OpenWrite(documentPath))
            {
                await content.CopyToAsync(file, cancellationToken);
            }

            var filesystemFileReference = FilesystemFileReference.Create(Path.GetFullPath(documentPath));
            return filesystemFileReference;
        }

        public async Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            var folderPath = Path.Join(projectPath, projectFolderId.Value.ToString());
            var documentPath = Path.Join(folderPath, id.Value.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (var file = File.OpenWrite(documentPath))
            {
                await content.CopyToAsync(file, cancellationToken);
            }

            return FilesystemFileReference.Create(Path.GetFullPath(documentPath));
        }

        public async Task<byte[]> GetFileContentAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not FilesystemFileReference fileSystemFileReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }

            try
            {
                await using (var fileStream = File.OpenRead(fileSystemFileReference.Filename))
                {
                    return await fileStream.ReadAllBytesAsync();
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new NotFoundException($"File ${fileSystemFileReference.Filename} not found");
            }
            catch (FileNotFoundException)
            {
                throw new NotFoundException($"File ${fileSystemFileReference.Filename} not found");
            }
        }

        public Task DeleteFileAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not FilesystemFileReference filesystemFileReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }

            if (File.Exists(filesystemFileReference.Filename))
            {
                File.Delete(filesystemFileReference.Filename);
            }
            
            return Task.CompletedTask;
        }
        
        public Task DeleteFilesAsync(ProjectId projectId, CancellationToken cancellationToken)
        {
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            if (Directory.Exists(projectPath))
            {
                Directory.Delete(projectPath, true);
            }

            return Task.CompletedTask;
        }

        public Task DeleteFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            CancellationToken cancellationToken)
        {
            var projectPath = Path.Join(_repositoryPath, projectId.Value.ToString());
            var folderPath = Path.Join(projectPath, projectFolderId.Value.ToString());
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }

            return Task.CompletedTask;
        }
    }
}
