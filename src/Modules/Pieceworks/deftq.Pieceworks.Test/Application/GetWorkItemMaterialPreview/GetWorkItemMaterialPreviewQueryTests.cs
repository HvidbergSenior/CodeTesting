using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetWorkItemMaterialPreview;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetWorkItemMaterialPreview
{
    public class GetWorkItemMaterialPreviewTests
    {
        [Fact]
        public async Task GivenFolderRoot_CanPreviewWorkItemMaterial()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var repository = new ProjectFolderRootInMemoryRepository();
            await repository.Add(folderRoot);
            var baseRateAndSupplementInMemoryRepository = new BaseRateAndSupplementRepository();

            var handler = new GetWorkItemMaterialPreviewQueryHandler(repository, baseRateAndSupplementInMemoryRepository);

            var query = GetWorkItemMaterialPreviewQuery.Create(folderRoot.ProjectId.Value, folderRoot.RootFolder.Id, new SystemTime().Today(), 1000,
                1,
                new List<SupplementOperation>(), new List<Supplement>());
            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Equal(1000m, resp.OperationTimeMilliseconds);
            Assert.Equal(1758.4m, resp.TotalWorkTimeMilliseconds);
        }

        [Fact]
        public async Task GivenFolderRoot_CanPreviewWorkItemMaterial_WithSupplementOperation()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var repository = new ProjectFolderRootInMemoryRepository();
            await repository.Add(folderRoot);
            var baseRateAndSupplementInMemoryRepository = new BaseRateAndSupplementRepository();

            var handler = new GetWorkItemMaterialPreviewQueryHandler(repository, baseRateAndSupplementInMemoryRepository);

            var supplementOperations = new List<SupplementOperation>
            {
                SupplementOperation.Create(SupplementOperation.SupplementOperationType.AmountRelated,1000, 1)
            };
            var query = GetWorkItemMaterialPreviewQuery.Create(folderRoot.ProjectId.Value, folderRoot.RootFolder.Id, new SystemTime().Today(), 2000,
                3, supplementOperations, new List<Supplement>());
            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Equal(2000m, resp.OperationTimeMilliseconds);
            Assert.Equal(12308.8m, resp.TotalWorkTimeMilliseconds);
        }
    }
}
