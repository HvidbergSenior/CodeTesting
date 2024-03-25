using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveDocument;
using deftq.Pieceworks.Domain.projectDocument;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectDocument
{
    public class RemoveProjectDocumentCommandTest
    {
        [Fact]
        public async Task RemoveProjectDocumentCommand_ShouldRemoveDocument()
        {
            // GIVEN
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            var subProjectFolder = Any.ProjectFolder();
            var projectDocument = Any.ProjectDocument(IFileReference.Empty());
            projectFolderRoot.AddFolder(projectFolder);
            projectFolderRoot.AddFolder(subProjectFolder, projectFolder.ProjectFolderId);
            subProjectFolder.AddDocumentReference(projectDocument.ProjectDocumentId, projectDocument.ProjectDocumentName, projectDocument.UploadedTimestamp);
            await repository.Add(projectFolderRoot);

            var handler = new RemoveDocumentCommandHandler(repository, uow);
            var cmd = RemoveProjectDocumentCommand.Create(projectFolderRoot.ProjectId.Value, subProjectFolder.ProjectFolderId.Value, projectDocument.ProjectDocumentId!.Value);
            
            // WHEN
            await handler.Handle(cmd, CancellationToken.None);

            // THEN
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            projectFolderRoot = await repository.GetById(projectFolderRoot.Id);
            var folder = projectFolderRoot.GetFolder(subProjectFolder.ProjectFolderId);
            Assert.Empty(folder!.Documents);
        }
        
    }
}
