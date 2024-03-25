using deftq.Pieceworks.Application.RemoveDocument;
using deftq.Pieceworks.Infrastructure.projectDocument;
using deftq.Pieceworks.Integration;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectDocument
{
    public class ProjectDocumentRemovedIntegrationEventHandlerTest
    {
        [Fact]
        public async Task GivenExternalFile_WhenDocumentIsRemoved_ExternalFileIsDeleted()
        {
            // Given
            var fileStorage = new FilesystemFileStorage();
            using var stream = new MemoryStream(new byte[] { 1, 2, 3, 4 });
            var projectId = Any.ProjectId();
            var projectFolderId = Any.ProjectFolderId();
            var fileReference = await fileStorage.StoreFileAsync(projectId, projectFolderId, Any.ProjectDocumentId(), Any.ProjectDocumentName(),
                stream, CancellationToken.None);
            var files = await fileStorage.ListFilesAsync(projectId, projectFolderId, CancellationToken.None);
            files.Should().ContainSingle();
            
            // When
            var integrationEventHandler = new ProjectDocumentRemovedIntegrationEventHandler(fileStorage);
            var documentRemovedIntegrationEvent = ProjectDocumentRemovedIntegrationEvent.Create(Guid.NewGuid(), DateTimeOffset.Now, (IExternalFileReference)fileReference);
            var projectDocumentRemovedIntegrationEvent = documentRemovedIntegrationEvent;
            await integrationEventHandler.Handle(projectDocumentRemovedIntegrationEvent, CancellationToken.None);

            // Then
            files = await fileStorage.ListFilesAsync(projectId, CancellationToken.None);
            files.Should().BeEmpty();
        }
    }
}
