using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.CopyProjectFolder;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CopyProjectFolder
{
    public class CopyProjectFolderCommandTest
    {
        [Fact]
        public async Task CopyingProjectFolderToASubFolder_ShouldReturnFolder()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var systemTime = new SystemTime();

            var folderRoot = Any.ProjectFolderRoot();

            var folder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("folder"),
                ProjectFolderDescription.Create("folder description"), ProjectFolderCreatedBy.Create("folder name", new DateTimeOffset()),
                FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());

            var subFolder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("subfolder"),
                ProjectFolderDescription.Create("subfolder description"), ProjectFolderCreatedBy.Create("subfolder name", new DateTimeOffset()),
                FolderRateAndSupplement.InheritAll(), ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());

            var destinationFolder = ProjectFolder.Create(ProjectFolderId.Create(Guid.NewGuid()), ProjectFolderName.Create("destinationfolder"),
                ProjectFolderDescription.Create("destinationfolder  description"),
                ProjectFolderCreatedBy.Create("destinationfolder name", new DateTimeOffset()), FolderRateAndSupplement.InheritAll(),
                ProjectFolderLock.Unlocked(), ProjectFolderExtraWork.Normal());
            folderRoot.AddFolder(folder);
            folderRoot.AddFolder(destinationFolder);
            folderRoot.AddFolder(subFolder, folder.ProjectFolderId);
            await repository.Add(folderRoot);

            var handler = new CopyProjectFolderCommandHandler(repository, uow, executionContext, systemTime);
            var cmd = CopyProjectFolderCommand.Create(folderRoot.ProjectId.Value, folder.ProjectFolderId.Value,
                destinationFolder.ProjectFolderId.Value);
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(repository.Entities);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);

            folderRoot = await repository.GetByProjectId(folderRoot.ProjectId.Value);

            //Assert folders still exists after copying
            Assert.Equal(folder.Name.Value, folderRoot.RootFolder.SubFolders[0].Name.Value);
            Assert.Equal(subFolder.Name.Value, folderRoot.RootFolder.SubFolders[0].SubFolders[0].Name.Value);

            //Assert folders are copied to destination
            Assert.Equal(folder.Name.Value, folderRoot.RootFolder.SubFolders[1].SubFolders[0].Name.Value);
            Assert.Equal(subFolder.Name.Value, folderRoot.RootFolder.SubFolders[1].SubFolders[0].SubFolders[0].Name.Value);
        }

        [Fact]
        public async Task CopyingUnknownProjectFolder_ShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var systemTime = new SystemTime();

            var projectFolderRoot = Any.ProjectFolderRoot();
            await repository.Add(projectFolderRoot);

            var handler = new CopyProjectFolderCommandHandler(repository, uow, executionContext, systemTime);
            var cmd = CopyProjectFolderCommand.Create(projectFolderRoot.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid());

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }

        [Fact]
        public async Task CopyingToUnknownProjectFolder_ShouldThrow()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var systemTime = new SystemTime();

            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            await repository.Add(folderRoot);

            var handler = new CopyProjectFolderCommandHandler(repository, uow, executionContext, systemTime);
            var cmd = CopyProjectFolderCommand.Create(folderRoot.ProjectId.Value, folder.ProjectFolderId.Value, Any.ProjectFolderId().Value);

            await Assert.ThrowsAsync<ProjectFolderNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
