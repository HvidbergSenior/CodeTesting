using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetWorkItemOperationPreview;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using Supplement = deftq.Pieceworks.Application.GetWorkItemOperationPreview.Supplement;

namespace deftq.Pieceworks.Test.Application.GetWorkItemOperationPreview
{
    public class GetWorkItemOperationPreviewTests
    {
        [Fact]
        public async Task GivenFolderRoot_CanPreviewWorkItemMaterial()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var repository = new ProjectFolderRootInMemoryRepository();
            await repository.Add(folderRoot);
            var baseRateAndSupplementInMemoryRepository = new BaseRateAndSupplementRepository();

            var handler = new GetWorkItemOperationPreviewQueryHandler(repository, baseRateAndSupplementInMemoryRepository);

            var query = GetWorkItemOperationPreviewQuery.Create(folderRoot.ProjectId.Value, folderRoot.RootFolder.Id, new SystemTime().Today(), 2000, 3,
                new List<Supplement>());
            var resp = await handler.Handle(query, CancellationToken.None);
            Assert.Equal(2000m, resp.OperationTimeMilliseconds);
            Assert.Equal(10550.4m, resp.TotalWorkTimeMilliseconds);
        }
    }
}
