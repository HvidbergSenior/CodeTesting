using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderLock;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.LockProjectFolder
{
    public class LockProjectFolderCommandHandlerTest
    {
        [Fact]
        public async Task LockProjectFolder()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);

            var handler = new UpdateLockProjectFolderCommandHandler(repository, uow, executionContext);

            await handler.Handle(
                UpdateProjectFolderLockCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value,
                    UpdateProjectFolderLockCommand.Lock.Locked, false), CancellationToken.None);

            folder = (await repository.GetByProjectId(projectFolderRoot.ProjectId.Value)).RootFolder.SubFolders[0];
            Assert.Equal(ProjectFolderLock.Locked(), folder.FolderLock);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
