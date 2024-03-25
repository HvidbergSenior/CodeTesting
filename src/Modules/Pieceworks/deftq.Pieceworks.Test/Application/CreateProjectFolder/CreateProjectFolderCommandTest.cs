using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.CreateProjectFolder;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CreateProjectFolder
{
    public class CreateProjectFolderCommandTest
    {
        [Fact]
        public async Task CreateProjectFolderCommand_ShouldCreateFolderInRootSuccessfully()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);

            var handler = new CreateProjectFolderCommandHandler(repository, uow, executionContext, new SystemTime());
            var cmd = CreateProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Any.ProjectFolderId().Value, "testFolder", "test description");
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            var projectRoot = await repository.GetByProjectId(projectFolderRoot.ProjectId.Value);
            Assert.Contains(projectRoot.RootFolder.SubFolders, f => f.Name == ProjectFolderName.Create("testFolder"));
        }

        [Fact]
        public async Task CreateProjectFolderCommand_ShouldCreateSubFolderSuccessfully()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var projectFolderRoot = Any.ProjectFolderRoot();
            var projectFolder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(projectFolder);
            await repository.Add(projectFolderRoot);

            var handler = new CreateProjectFolderCommandHandler(repository, uow, executionContext, new SystemTime());
            var cmd = CreateProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Any.ProjectFolderId().Value, "testSubFolder", "testSubFolderDescription", projectFolder.Id);

            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            var projectRoot = await repository.GetByProjectId(projectFolderRoot.ProjectId.Value);
            Assert.Equal(1, projectRoot.RootFolder.SubFolders.Count);
            Assert.Contains(projectRoot.RootFolder.SubFolders[0].SubFolders, f => f.Name == ProjectFolderName.Create("testSubFolder"));
        }
        
        [Fact]
        public async Task CreateProjectFolderCommand_UnknownProjectIdShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var handler = new CreateProjectFolderCommandHandler(repository, uow, executionContext, new SystemTime());
            var cmd = CreateProjectFolderCommand.Create(Any.ProjectId().Value, Any.ProjectFolderId().Value, "testFolder", "testFolderDescription");
            
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
        
        [Fact]
        public async Task CreateProjectFolderCommand_UnknownParentFolderIdShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);

            var handler = new CreateProjectFolderCommandHandler(repository, uow, executionContext, new SystemTime());
            var cmd = CreateProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Any.ProjectFolderId().Value, "testSubFolder", "testSubFolderDescription", Any.ProjectFolderId().Value);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
