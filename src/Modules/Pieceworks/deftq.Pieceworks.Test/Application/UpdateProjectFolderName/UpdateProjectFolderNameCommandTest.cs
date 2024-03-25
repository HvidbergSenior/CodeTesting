using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderName;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectFolderName
{
    public class UpdateProjectFolderNameCommandTest
    {
        [Fact]
        public async Task UpdateProjectFolderName_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);
            
            var handler = new UpdateProjectFolderNameCommandHandler(repository, uow, executionContext);
            var nameValue = Any.String(30);
            var cmd = UpdateProjectFolderNameCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value, nameValue);
            
            await handler.Handle(cmd, CancellationToken.None);

            folder = (await repository.GetByProjectId(projectFolderRoot.ProjectId.Value)).RootFolder.SubFolders[0];
            
            Assert.Equal(nameValue, folder.Name.Value);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
