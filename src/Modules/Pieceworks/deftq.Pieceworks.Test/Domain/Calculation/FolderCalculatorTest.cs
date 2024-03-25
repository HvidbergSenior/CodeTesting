using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public class FolderCalculatorTest
    {
        [Fact]
        public void GivenMissingFolderWork_ShouldThrowException()
        {
            var calculator = new FolderCalculator();

            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, Any.ProjectFolderId());

            Assert.Throws<InvalidOperationException>(() =>
                calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork }));
        }

        [Fact]
        public void GivenEmptyFolder_ShouldSumToZero()
        {
            var calculator = new FolderCalculator();
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(0, result.TotalPaymentExpression.Evaluate().Value);
            Assert.Equal(0, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenFolderWithMaterialWorkItem_ShouldSumToSameValue()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem = CreateMaterialWorkItem(20000, 7);
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(workItem.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value);
            Assert.Equal(workItem.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenFolderWithOperationWorkItem_ShouldSumToSameValue()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem = CreateOperationWorkItem(45000, 7);
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(workItem.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value);
            Assert.Equal(workItem.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenFolderWithMultipleWorkItems_ShouldSumValues()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem1 = CreateMaterialWorkItem(60000, 4);
            var workItem2 = CreateMaterialWorkItem(97000, 7);
            var workItem3 = CreateOperationWorkItem(45000, 11);
            folderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            folderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());
            folderWork.AddWorkItem(workItem3, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(workItem1.TotalPayment.Value + workItem2.TotalPayment.Value + workItem3.TotalPayment.Value,
                result.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(workItem1.TotalWorkTime.Value + workItem2.TotalWorkTime.Value + workItem3.TotalWorkTime.Value,
                result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenFolderWithNegativeWorkItems_ShouldGiveNegativeResult()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem1 = CreateMaterialWorkItem(60000, 4);
            var workItem2 = CreateMaterialWorkItem(-60000, 8);
            folderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            folderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.True(result.TotalPaymentExpression.Evaluate().Value < 0);
            Assert.Equal(workItem1.TotalPayment.Value + workItem2.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value, 4);

            Assert.True(result.TotalWorkTimeExpression.Evaluate().Value < 0);
            Assert.Equal(workItem1.TotalWorkTime.Value + workItem2.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenFolderWithNegativeWorkItems_ShouldGivePositiveResult()
        {
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem1 = CreateMaterialWorkItem(-60000, 4);
            var workItem2 = CreateMaterialWorkItem(60000, 8);
            folderWork.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            folderWork.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.True(result.TotalPaymentExpression.Evaluate().Value > 0);
            Assert.Equal(workItem1.TotalPayment.Value + workItem2.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value, 4);

            Assert.True(result.TotalWorkTimeExpression.Evaluate().Value > 0);
            Assert.Equal(workItem1.TotalWorkTime.Value + workItem2.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenFolderWithWorkInSubFolders_ShouldSumAllFolders()
        {
            var folderRoot = Any.ProjectFolderRoot();

            // Root
            // |- A1
            //    |- A2
            //    |- A3
            // |- B1
            //    |- B2
            var folderA1 = Any.ProjectFolder();
            var folderA2 = Any.ProjectFolder();
            var folderA3 = Any.ProjectFolder();
            var folderB1 = Any.ProjectFolder();
            var folderB2 = Any.ProjectFolder();
            folderRoot.AddFolder(folderA1);
            folderRoot.AddFolder(folderB1);
            folderRoot.AddFolder(folderA2, folderA1.ProjectFolderId);
            folderRoot.AddFolder(folderA3, folderA1.ProjectFolderId);
            folderRoot.AddFolder(folderB2, folderB1.ProjectFolderId);

            var folderWorkRoot = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, ProjectFolderRoot.RootFolderId);
            var folderWorkA1 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA1.ProjectFolderId);
            var folderWorkA2 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA2.ProjectFolderId);
            var folderWorkA3 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA3.ProjectFolderId);
            var folderWorkB1 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderB1.ProjectFolderId);
            var folderWorkB2 = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderB2.ProjectFolderId);

            var workItem = CreateOperationWorkItem(87000, 11);

            folderWorkA1.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            folderWorkA2.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            folderWorkA3.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            folderWorkB1.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            folderWorkB2.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());

            var calculator = new FolderCalculator();
            var folderWorks = new List<ProjectFolderWork>
            {
                folderWorkRoot,
                folderWorkA1,
                folderWorkA2,
                folderWorkA3,
                folderWorkB1,
                folderWorkB2
            };

            var result = calculator.CalculateTotalOperationTime(folderRoot, ProjectFolderRoot.RootFolderId, folderWorks);

            Assert.Equal(5 * workItem.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(5 * workItem.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenFolderWithWorkInSubFolders_ShouldOnlyIncludeSelectedFolderAndSubFolders()
        {
            var folderRoot = Any.ProjectFolderRoot();

            // Root
            // |- A
            // |- B
            var folderA = Any.ProjectFolder();
            var folderB = Any.ProjectFolder();
            folderRoot.AddFolder(folderA);
            folderRoot.AddFolder(folderB);

            var folderWorkRoot = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, ProjectFolderRoot.RootFolderId);
            var folderWorkA = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderA.ProjectFolderId);
            var folderWorkB = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folderB.ProjectFolderId);

            var workItemA = CreateOperationWorkItem(87000, 11);
            var workItemB = CreateOperationWorkItem(89000, 9);

            folderWorkA.AddWorkItem(workItemA, GetDefaultBaseRateAndSupplementProxy());
            folderWorkB.AddWorkItem(workItemB, GetDefaultBaseRateAndSupplementProxy());

            var calculator = new FolderCalculator();
            var folderWorks = new List<ProjectFolderWork> { folderWorkRoot, folderWorkA, folderWorkB };

            var resultA = calculator.CalculateTotalOperationTime(folderRoot, folderA.ProjectFolderId, folderWorks);
            Assert.Equal(workItemA.TotalPayment.Value, resultA.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(workItemA.TotalWorkTime.Value, resultA.TotalWorkTimeExpression.Evaluate().Value, 4);

            var resultB = calculator.CalculateTotalOperationTime(folderRoot, folderB.ProjectFolderId, folderWorks);
            Assert.Equal(workItemB.TotalPayment.Value, resultB.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(workItemB.TotalWorkTime.Value, resultB.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenFolderWithExtraWork_ShouldSumAsExtraWork()
        {
            // Root
            // |- folder (extra work)
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folder.MarkAsExtraWork();
            folderRoot.AddFolder(folder);
            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var workItem = CreateOperationWorkItem(45000, 7);
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId, new List<ProjectFolderWork> { folderWork });

            Assert.Equal(workItem.TotalPayment.Value, result.TotalPaymentExpression.Evaluate().Value);
            Assert.Equal(workItem.TotalWorkTime.Value, result.TotalWorkTimeExpression.Evaluate().Value);
            Assert.Equal(workItem.TotalPayment.Value, result.TotalExtraWorkPaymentExpression.Evaluate().Value);
            Assert.Equal(workItem.TotalWorkTime.Value, result.TotalExtraWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenFolderWithExtraWork_SubFoldersShouldSumAsExtraWork()
        {
            // Root
            // |- folder (extra work)
            //    |- sub folder1
            //    |- sub folder2 (extra work)
            //       |- sub folder3
            var folderRoot = Any.ProjectFolderRoot();
            var folder = Any.ProjectFolder();
            folder.MarkAsExtraWork();
            folderRoot.AddFolder(folder);
            var subFolder1 = Any.ProjectFolder();
            folderRoot.AddFolder(subFolder1, folder.ProjectFolderId);
            var subFolder2 = Any.ProjectFolder();
            folderRoot.AddFolder(subFolder2, folder.ProjectFolderId);
            var subFolder3 = Any.ProjectFolder();
            folderRoot.AddFolder(subFolder3, subFolder2.ProjectFolderId);

            var folderWork = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, folder.ProjectFolderId);
            var subFolder1Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, subFolder1.ProjectFolderId);
            var subFolder2Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, subFolder2.ProjectFolderId);
            var subFolder3Work = ProjectFolderWork.Create(Any.ProjectFolderWorkId(), folderRoot.ProjectId, subFolder3.ProjectFolderId);
            var workItem1 = CreateOperationWorkItem(45000, 7);
            var workItem2 = CreateOperationWorkItem(67000, 2);
            var workItem3 = CreateOperationWorkItem(26700, 12);
            subFolder1Work.AddWorkItem(workItem1, GetDefaultBaseRateAndSupplementProxy());
            subFolder2Work.AddWorkItem(workItem2, GetDefaultBaseRateAndSupplementProxy());
            subFolder3Work.AddWorkItem(workItem3, GetDefaultBaseRateAndSupplementProxy());
            var calculator = new FolderCalculator();

            var result = calculator.CalculateTotalOperationTime(folderRoot, folder.ProjectFolderId,
                new List<ProjectFolderWork> { folderWork, subFolder1Work, subFolder2Work, subFolder3Work });

            Assert.Equal(workItem1.TotalPayment.Value + workItem2.TotalPayment.Value + workItem3.TotalPayment.Value,
                result.TotalPaymentExpression.Evaluate().Value);
            
            Assert.Equal(workItem1.TotalWorkTime.Value + workItem2.TotalWorkTime.Value + workItem3.TotalWorkTime.Value,
                result.TotalWorkTimeExpression.Evaluate().Value);
            
            Assert.Equal(workItem1.TotalPayment.Value + workItem2.TotalPayment.Value + workItem3.TotalPayment.Value,
                result.TotalExtraWorkPaymentExpression.Evaluate().Value);
            
            Assert.Equal(workItem1.TotalWorkTime.Value + workItem2.TotalWorkTime.Value + workItem3.TotalWorkTime.Value,
                result.TotalExtraWorkTimeExpression.Evaluate().Value);
        }

        private static WorkItem CreateMaterialWorkItem(Decimal operationTimeMilliseconds, decimal amount)
        {
            return WorkItem.Create(WorkItemId.Empty(), CatalogMaterialId.Empty(), WorkItemDate.Today(), WorkItemUser.Empty(),
                WorkItemText.Empty(), WorkItemEanNumber.Empty(), WorkItemMountingCode.FromCode(3), WorkItemDuration.Create(operationTimeMilliseconds),
                WorkItemAmount.Create(amount), WorkItemUnit.Meter, new List<SupplementOperation>(), new List<Supplement>());
        }

        private static WorkItem CreateOperationWorkItem(Decimal operationTimeMilliseconds, decimal amount)
        {
            return WorkItem.Create(WorkItemId.Empty(), CatalogOperationId.Empty(), WorkItemOperationNumber.Empty(), WorkItemDate.Today(),
                WorkItemUser.Empty(), WorkItemText.Empty(), WorkItemDuration.Create(operationTimeMilliseconds), WorkItemAmount.Create(amount),
                new List<Supplement>());
        }
    }
}
