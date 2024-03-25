using deftq.BuildingBlocks.Domain;
using deftq.BuildingBlocks.Integration.Outbox;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Integration;

namespace deftq.Pieceworks.Application.RemoveDocument
{
    public class ProjectDocumentRemovedEventHandler : IDomainEventListener<ProjectDocumentRemovedDomainEvent>
    {
        private readonly IProjectDocumentRepository _projectDocumentRepository;
        private readonly IOutbox _outbox;
        private readonly ISystemTime _systemTime;

        public ProjectDocumentRemovedEventHandler(IProjectDocumentRepository projectDocumentRepository, IOutbox outbox, ISystemTime systemTime)
        {
            _projectDocumentRepository = projectDocumentRepository;
            _outbox = outbox;
            _systemTime = systemTime;
        }
        
        public async Task Handle(ProjectDocumentRemovedDomainEvent notification, CancellationToken cancellationToken)
        {
            var document = await _projectDocumentRepository.GetById(notification.ProjectDocumentId.Value, cancellationToken);
            if (document.FileReference is IExternalFileReference externalReference)
            {
                _outbox.Add(OutboxMessage.From(ProjectDocumentRemovedIntegrationEvent.Create(Guid.NewGuid(), _systemTime.Now(), externalReference)));    
            }

            await _projectDocumentRepository.Delete(document, cancellationToken);
            await _projectDocumentRepository.SaveChanges(cancellationToken);
        }
    }
}
