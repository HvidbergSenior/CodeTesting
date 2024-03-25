using deftq.Pieceworks.Application.GetProjectFolderSummation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.GetProjectFolderSummation
{
    public class GetProjectFolderSummationQueryTest
    {
        [Fact]
        public async Task Should_Return_ProjectFolderRoot()
        {
            var folderRootRepo = new ProjectFolderRootInMemoryRepository();
            var folderWorkRepo = new ProjectFolderWorkInMemoryRepository();

            // Root
            //  |- A
            //     |- A1
            //  |- B
            var folderRoot = Any.ProjectFolderRoot();
            var folderA = Any.ProjectFolder();
            var folderA1 = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();
            folderRoot.AddFolder(folderA);
            folderRoot.AddFolder(folderA1, folderA.ProjectFolderId);
            folderRoot.AddFolder(folderB);
            await folderRootRepo.Add(folderRoot);

            var folderWorkA = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA.ProjectFolderId);
            var folderWorkA1 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA1.ProjectFolderId);
            var folderWorkB = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderB.ProjectFolderId);
            folderWorkA.AddWorkItem(CreateMaterialWorkItem(12000, 2), GetDefaultBaseRateAndSupplementProxy());
            folderWorkA1.AddWorkItem(CreateMaterialWorkItem(9000, 5), GetDefaultBaseRateAndSupplementProxy());
            folderWorkB.AddWorkItem(CreateMaterialWorkItem(90000, 100), GetDefaultBaseRateAndSupplementProxy());
            await folderWorkRepo.Add(folderWorkA);
            await folderWorkRepo.Add(folderWorkA1);
            await folderWorkRepo.Add(folderWorkB);
                
            var query = GetProjectFolderSummationQuery.Create(folderRoot.ProjectId, folderA.ProjectFolderId);
            var handler = new GetProjectFolderSummationQueryHandler(folderRootRepo, folderWorkRepo);
            var response = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(42201.6m + 79128m, response.TotalWorkTimeMilliseconds, 4);
            Assert.Equal(2.52m + 4.72m, response.TotalPaymentDkr, 2);
        }
        
        private static WorkItem CreateMaterialWorkItem(Decimal operationTimeMilliseconds, decimal amount)
        {
            return WorkItem.Create(WorkItemId.Empty(), CatalogMaterialId.Empty(), WorkItemDate.Today(), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(operationTimeMilliseconds),
                WorkItemAmount.Create(amount), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());
        }
    }
}
