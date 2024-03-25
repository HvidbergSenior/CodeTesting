using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateFolderSupplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateFolderSupplements
{
    public class UpdateFolderSupplementsTest
    {
        [Fact]
        public async Task UpdateFolderSupplements_ShouldUpdateWith2Supplements()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);


            var handler = new UpdateFolderSupplementsCommandHandler(repository, uow, executionContext);
            var supplements = Any.FolderSupplements();
            var cmd = UpdateFolderSupplementsCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value, supplements);
            await handler.Handle(cmd, CancellationToken.None);

            var project = await repository.GetByProjectId(projectFolderRoot.ProjectId.Value);
            folder = project.RootFolder.SubFolders[0];

            Assert.NotNull(folder.FolderSupplements);
            Assert.Equal(2, folder.FolderSupplements.Count);
            Assert.Equal(supplements[0].SupplementNumber.Value, folder.FolderSupplements[0].SupplementNumber.Value);
            Assert.Equal(supplements[0].CatalogSupplementId.Value, folder.FolderSupplements[0].CatalogSupplementId.Value);
            Assert.Equal(supplements[0].SupplementText.Value, folder.FolderSupplements[0].SupplementText.Value);
            Assert.Equal(supplements[0].SupplementPercentage.Value, folder.FolderSupplements[0].SupplementPercentage.Value);
            Assert.Equal(supplements[1].SupplementNumber.Value, folder.FolderSupplements[1].SupplementNumber.Value);
            Assert.Equal(supplements[1].CatalogSupplementId.Value, folder.FolderSupplements[1].CatalogSupplementId.Value);
            Assert.Equal(supplements[1].SupplementText.Value, folder.FolderSupplements[1].SupplementText.Value);
            Assert.Equal(supplements[1].SupplementPercentage.Value, folder.FolderSupplements[1].SupplementPercentage.Value);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }

        [Fact]
        public async Task UpdateFolderSupplements_ShouldUpdateWithNoneSupplements()
        {
            var uow = new FakeUnitOfWork();
            var repository = new ProjectFolderRootInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var projectFolderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await repository.Add(projectFolderRoot);


            var handler = new UpdateFolderSupplementsCommandHandler(repository, uow, executionContext);
            var supplements = new List<FolderSupplement>();
            var cmd = UpdateFolderSupplementsCommand.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value, supplements);
            await handler.Handle(cmd, CancellationToken.None);

            folder = (await repository.GetByProjectId(projectFolderRoot.ProjectId.Value)).RootFolder.SubFolders[0];

            Assert.NotNull(folder.FolderSupplements);
            Assert.Equal(0, folder.FolderSupplements.Count);
            Assert.True(repository.SaveChangesCalled);
            Assert.True(uow.IsCommitted);
        }
    }
}
