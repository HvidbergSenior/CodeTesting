using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderExtraWork;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectFolderExtraWork
{
    public class UpdateProjectFolderExtraWorkCommandTest
    {
        [Fact]
        public async Task UpdateProjectFolderExtraWork_ShouldUpdate()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);

            var handler = new UpdateProjectFolderExtraWorkCommandHandler(repository, uow);
            var cmd = UpdateProjectFolderExtraWorkCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value,
                UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork);

            await handler.Handle(cmd, CancellationToken.None);

            folder = (await repository.GetByProjectId(projectFolderRoot.ProjectId.Value)).RootFolder.SubFolders[0];

            Assert.True(folder.IsExtraWork());
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
