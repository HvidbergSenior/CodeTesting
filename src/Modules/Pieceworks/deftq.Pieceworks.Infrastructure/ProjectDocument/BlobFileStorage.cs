using System.Web;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Baseline;
using deftq.BuildingBlocks.Exceptions;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;

namespace deftq.Pieceworks.Infrastructure.projectDocument
{
    public class BlobFileStorage : IFileStorage
    {
        private BlobServiceClient _client;
        private readonly string MetadataFilenameProperty = "Filename";
        
        public BlobFileStorage(string connectionString)
        {
            _client = new BlobServiceClient(connectionString);
        }
        
        public BlobFileStorage(string accountName, Uri accountUri, string accountKey)
        {
            _client = new BlobServiceClient(accountUri, new StorageSharedKeyCredential(accountName, accountKey));
        }

        public async Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId,
            CancellationToken cancellationToken)
        {
            var blobContainerClient = _client.GetBlobContainerClient(projectId.Value.ToString());
            List<IFileReference> documents = new List<IFileReference>();

            if (!await blobContainerClient.ExistsAsync(cancellationToken))
            {
                return new List<IFileReference>();
            }

            await foreach (var blob in blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None,
                               projectId.Value.ToString(), cancellationToken))
            {
                documents.Add(BlobFileReference.Create(projectId.Value.ToString(), blob.Name));
            }

            return documents;
        }

        public async Task<IEnumerable<IFileReference>> ListFilesAsync(ProjectId projectId,
            ProjectFolderId projectFolderId, CancellationToken cancellationToken)
        {
            var blobContainerClient = _client.GetBlobContainerClient(projectId.Value.ToString());
            List<IFileReference> documents = new List<IFileReference>();
            await foreach (var blob in blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None,
                               projectFolderId.Value.ToString(), cancellationToken))
            {
                documents.Add(BlobFileReference.Create(projectId.Value.ToString(), blob.Name));
            }

            return documents;
        }

        public async Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectDocumentId id,
            ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            return await StoreFileInternal(projectId, null, id, name, content, cancellationToken);
        }

        public async Task<IFileReference> StoreFileAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            return await StoreFileInternal(projectId, projectFolderId, id, name, content, cancellationToken);
        }

        private async Task<IFileReference> StoreFileInternal(ProjectId projectId, ProjectFolderId? projectFolderId,
            ProjectDocumentId id, ProjectDocumentName name, Stream content, CancellationToken cancellationToken)
        {
            var containerName = projectId.Value.ToString();
            var blobName = projectFolderId is null
                ? $"{projectId.Value}.{id.Value}"
                : $"{projectFolderId.Value}.{id.Value}";

            var blobContainerClient = _client.GetBlobContainerClient(containerName);
            if (!await blobContainerClient.ExistsAsync(cancellationToken))
            {
                await blobContainerClient.CreateAsync(PublicAccessType.None, null, null, cancellationToken);
            }

            await blobContainerClient.UploadBlobAsync(blobName, content, cancellationToken);
            var blockBlobClient = blobContainerClient.GetBlockBlobClient(blobName);
            var metadata = new Dictionary<string, string>(StringComparer.Ordinal) { [MetadataFilenameProperty] = HttpUtility.HtmlEncode(name.Value) };
            await blockBlobClient.SetMetadataAsync(metadata, null, cancellationToken);
            return BlobFileReference.Create(containerName, blobName);
        }

        public async Task<byte[]> GetFileContentAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not BlobFileReference blobFileReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }

            var blobContainerClient = _client.GetBlobContainerClient(blobFileReference.Container);
            var blobClient = blobContainerClient.GetBlobClient(blobFileReference.Blob);

            if (!await blobContainerClient.ExistsAsync(cancellationToken))
            {
                throw new NotFoundException(
                    $"Container {blobFileReference.Container}, blob {blobFileReference.Blob} not found");
            }

            try
            {
                await using (var blobStream =
                             await blobClient.OpenReadAsync(new BlobOpenReadOptions(false), (cancellationToken)))
                {
                    return await blobStream.ReadAllBytesAsync();
                }
            }
            catch (RequestFailedException e)
            {
                if (e.Status == 404)
                {
                    throw new NotFoundException(
                        $"Container ${blobFileReference.Container}, blob {blobFileReference.Blob} not found");
                }

                throw;
            }
        }

        public async Task DeleteFileAsync(IFileReference fileReference, CancellationToken cancellationToken)
        {
            if (fileReference is not BlobFileReference blobReference)
            {
                throw new NotSupportedException("Invalid file reference");
            }
            
            var blobContainerClient = _client.GetBlobContainerClient(blobReference.Container);
            await blobContainerClient.DeleteBlobAsync(blobReference.Blob, DeleteSnapshotsOption.None, null, cancellationToken);
        }
        
        public async Task DeleteFilesAsync(ProjectId projectId, CancellationToken cancellationToken)
        {
            var blobContainerClient = _client.GetBlobContainerClient(projectId.Value.ToString());
            await blobContainerClient.DeleteIfExistsAsync(null, cancellationToken);
        }

        public async Task DeleteFilesAsync(ProjectId projectId, ProjectFolderId projectFolderId,
            CancellationToken cancellationToken)
        {
            var blobContainerClient = _client.GetBlobContainerClient(projectId.Value.ToString());
            await foreach (var blob in blobContainerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None,
                               projectFolderId.Value.ToString(), cancellationToken))
            {
                await blobContainerClient.DeleteBlobIfExistsAsync(blob.Name, DeleteSnapshotsOption.None, null,
                    cancellationToken);
            }
        }
    }
}
