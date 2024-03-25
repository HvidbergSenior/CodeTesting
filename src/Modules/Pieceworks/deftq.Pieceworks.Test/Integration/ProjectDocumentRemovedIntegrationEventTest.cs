using deftq.BuildingBlocks.Integration.Outbox;
using deftq.Pieceworks.Infrastructure.projectDocument;
using deftq.Pieceworks.Integration;
using Xunit;

namespace deftq.Pieceworks.Test.Integration
{
    public class ProjectDocumentRemovedIntegrationEventTest
    {
        [Fact]
        public void Serialize()
        {
            var integrationEvent =
                ProjectDocumentRemovedIntegrationEvent.Create(Guid.NewGuid(), DateTimeOffset.UtcNow, FilesystemFileReference.Create("dummy"));

            var outboxMessage = OutboxMessage.From(integrationEvent);

            var serializer = new OutboxMessageSerializer();
            var obj = serializer.Deserialize(outboxMessage.Payload, outboxMessage.MessageType);

            Assert.NotNull(obj);
            Assert.IsType<ProjectDocumentRemovedIntegrationEvent>(obj);
            Assert.Equal(integrationEvent.Id, ((ProjectDocumentRemovedIntegrationEvent)obj!).Id);
        }
    }
}
