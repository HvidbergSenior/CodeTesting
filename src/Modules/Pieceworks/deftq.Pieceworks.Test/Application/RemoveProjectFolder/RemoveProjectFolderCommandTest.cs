using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProjectFolder;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectFolder
{
    public class RemoveProjectFolderCommandTest
    {
        [Fact]
        public async Task RemoveProjectFolderCommand_ShouldNotReturnFolder()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);
            await repository.Add(projectFolderRoot);

            var handler = new RemoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = RemoveProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, projectFolder.ProjectFolderId.Value);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            projectFolderRoot = await repository.GetById(projectFolderRoot.Id);
            Assert.DoesNotContain(projectFolder, projectFolderRoot.RootFolder.SubFolders);
        }

        [Fact]
        public async Task RemoveProjectFolderCommand_ShouldRemoveRecursively()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);
            var subProjectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(subProjectFolder, projectFolder.ProjectFolderId);
            await repository.Add(projectFolderRoot);
            
            var handler = new RemoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = RemoveProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, projectFolder.ProjectFolderId.Value);
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            projectFolderRoot = await repository.GetById(projectFolderRoot.Id);
            Assert.Empty(projectFolderRoot.RootFolder.SubFolders);
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.GetFolder(projectFolder.ProjectFolderId));
            Assert.Throws<ProjectFolderNotFoundException>(() => projectFolderRoot.GetFolder(subProjectFolder.ProjectFolderId));
        }
        
        [Fact]
        public async Task RemoveUnknownProjectFolderCommand_ShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);
            
            var handler = new RemoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = RemoveProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Any.ProjectFolderId().Value);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
