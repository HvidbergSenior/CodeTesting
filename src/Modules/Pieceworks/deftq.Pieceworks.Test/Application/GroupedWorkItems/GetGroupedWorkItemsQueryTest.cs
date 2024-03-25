using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.GroupedWorkItems
{
    public class GetGroupedWorkItemsQueryTest
    {
        private readonly ProjectFolderRootInMemoryRepository _folderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;

        public GetGroupedWorkItemsQueryTest()
        {
            _folderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public async Task WhenProjectHasOneFolderWithOneWorkItem_GetGroupedItems_ExtractsCorrectResult()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            // Create project folder
            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);
            await _folderRootRepository.Add(projectFolderRoot);

            // Create work item and add to folder
            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Empty(), WorkItemDate.Empty(), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(7500), WorkItemAmount.Create(5),
                WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());
            
            var folder1Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectFolderRoot.ProjectId, folder.ProjectFolderId);
            folder1Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            await _folderWorkRepository.Add(folder1Work);

            var query = GetGroupedWorkItemsQuery.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value, 10);
            var executionContext = new FakeExecutionContext();
            var handler = new GetGroupedWorkItemsQueryHandler(_folderRootRepository, _folderWorkRepository, executionContext);

            var getGroupedWorkItemsQueryResponse = await handler.Handle(query, CancellationToken.None);
            
            Assert.Equal(5, getGroupedWorkItemsQueryResponse.GroupedWorkItems[0].Amount);
            Assert.Equal(3.93m, getGroupedWorkItemsQueryResponse.GroupedWorkItems[0].PaymentDkr, 2);
        }
    }
}
