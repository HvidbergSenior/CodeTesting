using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Infrastructure.projectDocument;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Pieceworks.Test.Infrastructure.projectDocument
{
    public class FileStorageTest
    {
        private readonly ITestOutputHelper _output;
        
        // Blob file storage
        private const string _storageAccount = "FILL_IN";
        private const string _storageKey = "FILL_IN";
        private readonly Uri _accountUri = new Uri($"https://{_storageAccount}.blob.core.windows.net/");
        private BlobFileStorage _blobFileStorage;
        
        // Filesystem file storage
        private FilesystemFileStorage _filesystemFileStorage = new FilesystemFileStorage();
        
        // In memory file storage
        private InMemoryFileStorage _inMemoryFileStorage = new InMemoryFileStorage();
        
        // List of storage providers to test
        private IEnumerable<IFileStorage> GetStorageTypes()
        {
            return new IFileStorage[] { _blobFileStorage, _filesystemFileStorage, _inMemoryFileStorage };
        }
        
        public FileStorageTest(ITestOutputHelper output)
        {
            _output = output;
            _blobFileStorage = new BlobFileStorage(_storageAccount, _accountUri, _storageKey);
        }

        [Fact(Skip = "manual")]
        public async Task StoreFileOnProject()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    await storage.StoreFileAsync(projectId, Any.ProjectDocumentId(), ProjectDocumentName.Create("building1.pdf"), stream, CancellationToken.None);    
                }
                var files = await storage.ListFilesAsync(projectId, CancellationToken.None);
            
                Assert.Single(files);    
            }
        }
        
        [Fact(Skip = "manual")]
        public async Task StoreFileOnProjectFolder()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                var projectFolderId = Any.ProjectFolderId();
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    await storage.StoreFileAsync(projectId, projectFolderId, Any.ProjectDocumentId(), ProjectDocumentName.Create("building1.pdf"), stream, CancellationToken.None);    
                }
                Assert.Empty(await storage.ListFilesAsync(projectId, CancellationToken.None));
                Assert.Single(await storage.ListFilesAsync(projectId, projectFolderId, CancellationToken.None));    
            }
        }
        
        [Fact(Skip = "manual")]
        public async Task DeleteFileOnProject()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    await storage.StoreFileAsync(projectId, Any.ProjectDocumentId(), ProjectDocumentName.Create("file_to_delete.pdf"), stream, CancellationToken.None);    
                }

                Assert.Single(await storage.ListFilesAsync(projectId, CancellationToken.None));
                await storage.DeleteFilesAsync(projectId, CancellationToken.None);
                Assert.Empty(await storage.ListFilesAsync(projectId, CancellationToken.None));    
            }
        }
        
        [Fact(Skip = "manual")]
        public async Task DeleteFileOnProjectFolder()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                var projectFolderId = Any.ProjectFolderId();
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    await storage.StoreFileAsync(projectId, projectFolderId, Any.ProjectDocumentId(), ProjectDocumentName.Create("file_to_delete.pdf"), stream, CancellationToken.None);    
                }

                Assert.Single(await storage.ListFilesAsync(projectId, projectFolderId, CancellationToken.None));
                await storage.DeleteFilesAsync(projectId, projectFolderId, CancellationToken.None);
                Assert.Empty(await storage.ListFilesAsync(projectId, projectFolderId, CancellationToken.None));    
            }
        }
        
        [Fact(Skip = "manual")]
        public async Task GetFile()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                var projectFolderId = Any.ProjectFolderId();
                IFileReference reference;
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    reference = await storage.StoreFileAsync(projectId, projectFolderId, Any.ProjectDocumentId(), ProjectDocumentName.Create("file_to_delete.pdf"), stream, CancellationToken.None);
                }

                using (var outputStream = new MemoryStream())
                {
                    var bytes = await storage.GetFileContentAsync(reference, CancellationToken.None);
                    Assert.Equal(new byte[] { 1, 2, 3, 4, }, bytes);
                }
            }
        }
        
        [Fact(Skip = "manual")]
        public async Task GetDeletedFile()
        {
            foreach (var storage in GetStorageTypes())
            {
                var projectId = Any.ProjectId();
                var projectFolderId = Any.ProjectFolderId();
                IFileReference reference;
                using (var stream = new MemoryStream(new byte[] { 1, 2, 3, 4, }))
                {
                    reference = await storage.StoreFileAsync(projectId, projectFolderId, Any.ProjectDocumentId(), ProjectDocumentName.Create("file_to_delete.pdf"), stream, CancellationToken.None);
                }

                await storage.DeleteFilesAsync(projectId, CancellationToken.None);
                
                using (var outputStream = new MemoryStream())
                {
                    await Assert.ThrowsAsync<NotFoundException>(async () => await storage.GetFileContentAsync(reference, CancellationToken.None));
                }
            }
        }
    }
}
