using deftq.BuildingBlocks.Integration;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Integration;

namespace deftq.Pieceworks.Application.RemoveDocument
{
    public class ProjectDocumentRemovedIntegrationEventHandler : IIntegrationEventListener<ProjectDocumentRemovedIntegrationEvent>
    {
        private readonly IFileStorage _fileStorage;

        public ProjectDocumentRemovedIntegrationEventHandler(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }
        
        public async Task Handle(ProjectDocumentRemovedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            if (notification.ExternalFileReference is IFileReference fileReference)
            {
                await _fileStorage.DeleteFileAsync(fileReference, cancellationToken);
            }
        }
    }
}
