using deftq.BuildingBlocks.Integration;

namespace deftq.Pieceworks.Integration
{
    public sealed class ProjectDocumentRemovedIntegrationEvent : IntegrationEvent
    {
        public IExternalFileReference ExternalFileReference { get; private set; }

        private ProjectDocumentRemovedIntegrationEvent() : base(Guid.NewGuid(), DateTimeOffset.UtcNow)
        {
            ExternalFileReference = new EmptyExternalFileReference();
        }
        
        private ProjectDocumentRemovedIntegrationEvent(Guid id, DateTimeOffset occurredOn, IExternalFileReference externalFileReference) : base(id, occurredOn)
        {
            ExternalFileReference = externalFileReference;
        }

        public static ProjectDocumentRemovedIntegrationEvent Create(Guid id, DateTimeOffset occurredOn, IExternalFileReference externalFileReference)
        {
            return new ProjectDocumentRemovedIntegrationEvent(id, occurredOn, externalFileReference);
        }

        private class EmptyExternalFileReference : IExternalFileReference
        {
            
        }
    }

    public interface IExternalFileReference
    {
        
    }
}
