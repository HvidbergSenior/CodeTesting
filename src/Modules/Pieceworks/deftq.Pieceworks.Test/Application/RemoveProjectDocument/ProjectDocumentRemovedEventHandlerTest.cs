using deftq.BuildingBlocks.Integration.Outbox;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.RemoveDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using deftq.Pieceworks.Infrastructure.projectDocument;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectDocument
{
    public class ProjectDocumentRemovedEventHandlerTest
    {
        [Fact]
        public async Task GivenProjectDocument_WhenRemovingDocument_IntegrationEventShouldBeRaised()
        {
            // Given
            var ctx = CancellationToken.None;
            var projectDocumentInMemoryRepository = new ProjectDocumentInMemoryRepository();
            var projectDocument = Any.ProjectDocument(new BlobFileReference());
            await projectDocumentInMemoryRepository.Add(projectDocument, ctx);
            var fakeOutbox = new FakeOutbox();
            var eventHandler = new ProjectDocumentRemovedEventHandler(projectDocumentInMemoryRepository, fakeOutbox, new SystemTime());

            // When
            var documentRemovedEvent = ProjectDocumentRemovedDomainEvent.Create(projectDocument.ProjectId, ProjectFolderId.Empty(), projectDocument.ProjectDocumentId);
            await eventHandler.Handle(documentRemovedEvent, ctx);
            
            // Then
            projectDocumentInMemoryRepository.Entities.Should().BeEmpty();
            projectDocumentInMemoryRepository.SaveChangesCalled.Should().BeTrue();
            fakeOutbox.Messages.Should().ContainSingle();
        }
    }
}
