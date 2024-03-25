using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using Xunit;
using Supplement = deftq.Pieceworks.Domain.FolderWork.Supplements.Supplement;
using SupplementOperation = deftq.Pieceworks.Domain.FolderWork.Supplements.SupplementOperation;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public class GroupedWorkItemsCalculatorTest
    {
        [Fact]
        public void ShouldCalculateWorkItemsOnFolderWithNoSubFolders()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(12), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var workItem2 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Create("0000000000000"), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(100),
                WorkItemAmount.Create(5), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);

            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);
            folderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });


            Assert.Equal(15, result[0].Amount);
            Assert.Equal(10.54m, result[0].PaymentDkr, 2);
        }

        [Fact]
        public void ShouldCalculateGroupedWorkItemsOnRootFolder()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderRoot.RootFolder.ProjectFolderId);

            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(12), WorkItemDuration.Create(7500),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var workItem2 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Create("0000000000000"), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(100),
                WorkItemAmount.Create(5), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplementProxy = GetDefaultBaseRateAndSupplementProxy();

            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);
            folderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folderRoot.RootFolder.ProjectFolderId,
                new List<ProjectFolderWork> { folderWork });

            Assert.Equal(15, result[0].Amount);
            Assert.Equal(7.92m, result[0].PaymentDkr, 2);
        }

        [Fact]
        public void ShouldIncludeSubfoldersWhenCalculatingGroupedWorkItemsTotalAmount()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(12), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var workItem2 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Create("0000000000000"), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(100),
                WorkItemAmount.Create(5), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);

            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);
            folderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);

            var folder2 = Any.ProjectFolder();
            folderRoot.AddFolder(folder2, folder.ProjectFolderId);

            var workItem3 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(15), WorkItemDuration.Create(12000),
                WorkItemAmount.Create(100), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var folderWork2 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder2.ProjectFolderId);
            var folder2BaseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2);
            
            folderWork2.AddWorkItem(workItem3, folder2BaseRateAndSupplementProxy);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork, folderWork2 });

            Assert.Equal(115, result[0].Amount);
            Assert.Equal(136.41m, result[0].PaymentDkr, 2);
        }

        [Fact]
        public void ShouldCalculateGroupedWorkItemsOnEmptyFolder_ResultShouldBeZero()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void ShouldCalculateNegativeValuesInGroupedWorkItem()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(12), WorkItemDuration.Create(-7200),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);

            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(-7.55m, result[0].PaymentDkr, 2);
        }

        [Fact]
        public void ShouldOnlyCalculateWorkItemsInRelevantFolders()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var workItem = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(12), WorkItemDuration.Create(10000),
                WorkItemAmount.Create(10), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var workItem2 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Create("0000000000000"), WorkItemMountingCode.FromCode(11), WorkItemDuration.Create(100),
                WorkItemAmount.Create(5), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);

            folderWork.AddWorkItem(workItem, baseRateAndSupplementProxy);
            folderWork.AddWorkItem(workItem2, baseRateAndSupplementProxy);

            var folder2 = Any.ProjectFolder();
            folderRoot.AddFolder(folder2);

            var workItem3 = WorkItem.Create(WorkItemId.Create(Guid.NewGuid()), CatalogMaterialId.Create(Guid.NewGuid()),
                WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Now)), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(15), WorkItemDuration.Create(12000),
                WorkItemAmount.Create(100), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());

            var folderWork2 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder2.ProjectFolderId);
            var folder2BaseRateAndSupplementProxy = new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder);
            
            folderWork2.AddWorkItem(workItem3, folder2BaseRateAndSupplementProxy);

            var calc = new GroupedWorkItemsCalculator();
            var result = calc.CalculateGroupedWorkItems(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork, folderWork2 });

            Assert.Equal(15, result[0].Amount);
            Assert.Equal(10.54m, result[0].PaymentDkr, 2);
        }
    }
}
