using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Application.GetWorkItems
{
    public class GetWorkItemsQueryTests
    {
        private readonly ProjectFolderRootInMemoryRepository _folderRootRepository;
        private readonly ProjectFolderWorkInMemoryRepository _folderWorkRepository;

        public GetWorkItemsQueryTests()
        {
            _folderRootRepository = new ProjectFolderRootInMemoryRepository();
            _folderWorkRepository = new ProjectFolderWorkInMemoryRepository();
        }

        [Fact]
        public async Task Should_Get_Work_Items_From_Folder()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);

            var folder2 = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder2);

            var workItem = Any.WorkItem();
            var workItem2 = Any.WorkItem();
            var folder1Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectFolderRoot.ProjectId, folder.ProjectFolderId);
            var folder2Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectFolderRoot.ProjectId, folder2.ProjectFolderId);
            folder1Work.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            folder2Work.AddWorkItem(workItem2, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder2));
            await _folderWorkRepository.Add(folder1Work);
            await _folderWorkRepository.Add(folder2Work);

            await _folderRootRepository.Add(projectFolderRoot);
            var handler = new GetWorkItemsQueryHandler(_folderWorkRepository);

            var folderQuery = GetWorkItemsQuery.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value);
            var folderResponse = await handler.Handle(folderQuery, CancellationToken.None);
            Assert.Equal(projectFolderRoot.ProjectId.Value, folderResponse.ProjectId);
            Assert.Equal(folder.ProjectFolderId.Value, folderResponse.ProjectFolderId);
            Assert.Equal(workItem.WorkItemId.Value, folderResponse.WorkItems[0].WorkItemId);
            Assert.Equal(workItem.Amount.Value, folderResponse.WorkItems[0].WorkItemAmount);
            Assert.Equal(workItem.WorkItemMaterial?.EanNumber.Value, folderResponse.WorkItems[0].WorkItemMaterial?.WorkItemEanNumber);
            Assert.Equal(workItem.WorkItemMaterial?.MountingCode.MountingCode, folderResponse.WorkItems[0].WorkItemMaterial?.WorkItemMountingCode);
            Assert.Equal(workItem.WorkItemMaterial?.MountingCode.Text, folderResponse.WorkItems[0].WorkItemMaterial?.WorkItemMountingCodeText);

            var folder2Query = GetWorkItemsQuery.Create(projectFolderRoot.ProjectId.Value, folder2.ProjectFolderId.Value);
            var folder2Response = await handler.Handle(folder2Query, CancellationToken.None);
            Assert.Equal(projectFolderRoot.ProjectId.Value, folder2Response.ProjectId);
            Assert.Equal(folder2.ProjectFolderId.Value, folder2Response.ProjectFolderId);
            Assert.Equal(workItem2.WorkItemId.Value, folder2Response.WorkItems[0].WorkItemId);
            Assert.Equal(workItem2.Amount.Value, folder2Response.WorkItems[0].WorkItemAmount);
            Assert.Equal(workItem2.WorkItemMaterial?.EanNumber.Value, folder2Response.WorkItems[0].WorkItemMaterial?.WorkItemEanNumber);
            Assert.Equal(workItem2.WorkItemMaterial?.MountingCode.MountingCode, folder2Response.WorkItems[0].WorkItemMaterial?.WorkItemMountingCode);
            Assert.Equal(workItem2.WorkItemMaterial?.MountingCode.Text, folder2Response.WorkItems[0].WorkItemMaterial?.WorkItemMountingCodeText);
        }

        [Fact]
        public async Task Should_Get_SupplementOperations_On_WorkItem()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectFolderRoot.ProjectId, folder.ProjectFolderId);

            var supplementOperation1 = SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("82C13696-6DEA-4829-B9F2-EF8F403B1E0B")),
                CatalogSupplementOperationId.Create(Guid.Parse("6CCF61F2-DF93-4309-91D7-9947BA3C0E22")),
                SupplementOperationText.Create("Move stuff around"), SupplementOperationType.AmountRelated(),
                SupplementOperationTime.Create(13000), SupplementOperationAmount.Create(9));

            var supplementOperation2 = SupplementOperation.Create(SupplementOperationId.Create(Guid.Parse("7D60482C-68CF-4738-8735-671E9EB98367")),
                CatalogSupplementOperationId.Create(Guid.Parse("7DDE845F-55FD-43A2-ABEA-282AE27CB412")),
                SupplementOperationText.Create("Drill some holes"), SupplementOperationType.UnitRelated(),
                SupplementOperationTime.Create(22000), SupplementOperationAmount.Create(23));

            var supplementOperations = new List<SupplementOperation> { supplementOperation1, supplementOperation2 };

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), Any.WorkItemDuration(), Any.WorkItemAmount(), Any.WorkItemUnit(), supplementOperations,
                new List<Supplement>());
            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            await _folderWorkRepository.Add(folderWork);

            await _folderRootRepository.Add(projectFolderRoot);
            var handler = new GetWorkItemsQueryHandler(_folderWorkRepository);

            var folderQuery = GetWorkItemsQuery.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value);
            var folderResponse = await handler.Handle(folderQuery, CancellationToken.None);

            Assert.Equal(2, folderResponse.WorkItems[0].WorkItemMaterial?.SupplementOperations.Count);
            var op1 = folderResponse.WorkItems[0].WorkItemMaterial?.SupplementOperations
                .First(op => op.SupplementOperationId == Guid.Parse("82C13696-6DEA-4829-B9F2-EF8F403B1E0B"));
            var op2 = folderResponse.WorkItems[0].WorkItemMaterial?.SupplementOperations
                .First(op => op.SupplementOperationId == Guid.Parse("7D60482C-68CF-4738-8735-671E9EB98367"));
            Assert.Equal("Move stuff around", op1?.Text);
            Assert.Equal("Drill some holes", op2?.Text);
            Assert.Equal(9, op1?.Amount);
            Assert.Equal(23, op2?.Amount);
            Assert.Equal(13000, op1?.OperationTimeMilliseconds);
            Assert.Equal(22000, op2?.OperationTimeMilliseconds);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.AmountRelated, op1?.OperationType);
            Assert.Equal(WorkItemSupplementOperationResponse.WorkItemSupplementOperationType.UnitRelated, op2?.OperationType);
        }

        [Fact]
        public async Task Should_Get_Supplements_On_WorkItem()
        {
            var projectFolderRoot = Any.ProjectFolderRoot();

            var folder = Any.ProjectFolder();
            projectFolderRoot.AddFolder(folder);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), projectFolderRoot.ProjectId, folder.ProjectFolderId);

            var supplementId1 = SupplementId.Create(Guid.Parse("6552C7B4-DD4D-460A-B0FB-ABE924055AFB"));
            var supplement1 = Supplement.Create(supplementId1, CatalogSupplementId.Empty(),
                SupplementNumber.Create("bah30"), SupplementText.Create("text1"), SupplementPercentage.Empty());

            var supplementId2 = SupplementId.Create(Guid.Parse("3811D568-1997-4919-99A1-01BFEFD90A19"));
            var supplement2 = Supplement.Create(supplementId2, CatalogSupplementId.Empty(), SupplementNumber.Create("bah40"),
                SupplementText.Create("text2"), SupplementPercentage.Empty());

            var supplements = new List<Supplement> { supplement1, supplement2 };

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), Any.WorkItemDuration(), Any.WorkItemAmount(), Any.WorkItemUnit(), new List<SupplementOperation>(),
                supplements);
            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            
            await _folderWorkRepository.Add(folderWork);
            await _folderRootRepository.Add(projectFolderRoot);
            
            var handler = new GetWorkItemsQueryHandler(_folderWorkRepository);

            var folderQuery = GetWorkItemsQuery.Create(projectFolderRoot.ProjectId.Value, folder.ProjectFolderId.Value);
            var folderResponse = await handler.Handle(folderQuery, CancellationToken.None);

            Assert.Equal(2, folderResponse.WorkItems[0].Supplements.Count);
            var supp1 = folderResponse.WorkItems[0].Supplements
                .First(op => op.SupplementId == Guid.Parse("6552C7B4-DD4D-460A-B0FB-ABE924055AFB"));
            var supp2 = folderResponse.WorkItems[0].Supplements
                .First(op => op.SupplementId == Guid.Parse("3811D568-1997-4919-99A1-01BFEFD90A19"));
            Assert.Equal("text1", supp1.SupplementText);
            Assert.Equal("text2", supp2.SupplementText);
            Assert.Equal("bah30", supp1.SupplementNumber);
            Assert.Equal("bah40", supp2.SupplementNumber);
        }
    }
}
