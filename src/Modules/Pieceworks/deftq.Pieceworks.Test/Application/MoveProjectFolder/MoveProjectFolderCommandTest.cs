using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.MoveProjectFolder;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.MoveProjectFolder
{
    public class MoveProjectFolderCommandTest
    {
        [Fact]
        public async Task GivenRootFolder_MovingItToDestinationFolder_ShouldNotReturnFolder()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var folderToMove = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(folderToMove);
            await repository.Add(folderRoot);

            var handler = new MoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = MoveProjectFolderCommand.Create(folderRoot.ProjectId.Value, folder.ProjectFolderId.Value, folderToMove.ProjectFolderId.Value);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            folderRoot = await repository.GetById(folderRoot.Id);
            Assert.DoesNotContain(folder, folderRoot.RootFolder.SubFolders);
            Assert.Contains(folderRoot.RootFolder.SubFolders, f => f.ProjectFolderId == folderToMove.ProjectFolderId);
        }

        [Fact]
        public async Task GivenSubFolder_MovingItToRoot_ShouldReturnFolder()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            var subFolderToMove = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(subFolderToMove, folder.ProjectFolderId);
            await repository.Add(folderRoot);
            
            var handler = new MoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = MoveProjectFolderCommand.Create(folderRoot.ProjectId.Value, subFolderToMove.ProjectFolderId.Value, ProjectFolderRoot.RootFolderId.Value);
            await handler.Handle(cmd, CancellationToken.None);
            
            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            folderRoot = await repository.GetById(folderRoot.Id);
            Assert.DoesNotContain(subFolderToMove, folderRoot.GetFolder(folder.ProjectFolderId)!.SubFolders);
            Assert.Contains(folderRoot.RootFolder.SubFolders, f => f.ProjectFolderId == subFolderToMove.ProjectFolderId);
        }
        
        [Fact]
        public async Task MovingUnknownProjectFolder_ShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);
            
            var handler = new MoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = MoveProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid());

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
        
        [Fact]
        public async Task MovingToUnknownProjectFolder_ShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            await repository.Add(folderRoot);
            
            var handler = new MoveProjectFolderCommandHandler(repository, uow, executionContext);
            var cmd = MoveProjectFolderCommand.Create(folderRoot.ProjectId.Value, folder.ProjectFolderId.Value, Any.ProjectFolderId().Value);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
