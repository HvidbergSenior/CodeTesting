using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderDescription;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectFolderDescription
{
    public class UpdateProjectFolderDescriptionCommandTest
    {
        [Fact]
        public async Task UpdateProjectFolderDescription_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);
            
            var handler = new UpdateProjectFolderDescriptionCommandHandler(repository, uow, executionContext);
            var descriptionValue = Any.String(30);
            var cmd = UpdateProjectFolderDescriptionCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value, descriptionValue);
            await handler.Handle(cmd, CancellationToken.None);
            
            folder = (await repository.GetByProjectId(projectFolderRoot.ProjectId.Value)).RootFolder.SubFolders[0];
            
            Assert.Equal(descriptionValue, folder.Description.Value);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        } 
    }
}
