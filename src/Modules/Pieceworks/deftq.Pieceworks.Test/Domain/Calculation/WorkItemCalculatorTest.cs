using System.Collections.ObjectModel;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using deftq.Pieceworks.Domain.projectFolderRoot;
using deftq.Pieceworks.Domain.projectFolderRoot.BaseRateAndSupplement;
using Xunit;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.Calculation
{
    public class WorkItemCalculatorTest
    {
        [Fact]
        public void GivenInvalidValues_ThrowsException()
        {
            SupplementOperationTime.Create(-1); // Negative supplement operation time should be supported 
            Assert.Throws<ArgumentOutOfRangeException>(() => SupplementOperationAmount.Create(-1));
            WorkItemDuration.Create(-1); // Negative operation time should be supported
            WorkItemAmount.Create(0);
            WorkItemAmount.Create(1); // Zero or positive amount should be supported
            Assert.Throws<ArgumentOutOfRangeException>(() => WorkItemAmount.Create(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBaseRateAndSupplement(-1, 4, 8, 220, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBaseRateAndSupplement(60, -1, 8, 220, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBaseRateAndSupplement(60, 4, -1, 220, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBaseRateAndSupplement(60, 4, 8, -1, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateBaseRateAndSupplement(60, 4, 8, 220, -1));
            Assert.Throws<ArgumentOutOfRangeException>(() => SupplementPercentage.Create(-1));
        }

        [Fact]
        public void CalculateCombinedSupplementPercentage()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(25000), WorkItemAmount.Create(30), WorkItemUnit.Meter,
                new Collection<SupplementOperation>(), new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(0.7584m, result.CombinedSupplementExpression.Evaluate().Value);
        }

        [Fact]
        public void CalculateTotalOperationTime()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(223200), WorkItemAmount.Create(3), WorkItemUnit.Meter,
                new List<SupplementOperation>(), new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(1177424.64m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void ZeroAmountShouldBeSupported()
        {
            var supplementOperation = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(223200), WorkItemAmount.Create(0), WorkItemUnit.Meter,
                new List<SupplementOperation> { supplementOperation }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(0m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void ZeroAmountWithSupplementOperationShouldBeSupported()
        {
            var amountRelatedOp = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.AmountRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var unitRelatedOp = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(223200), WorkItemAmount.Create(0), WorkItemUnit.Meter,
                new List<SupplementOperation> { unitRelatedOp, amountRelatedOp }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(0m, result.TotalWorkTimeExpression.Evaluate().Value);
            Assert.Equal(0m, result.TotalPaymentExpression.Evaluate().Value);
        }

        [Fact]
        public void IncludeAmountRelatedSupplementOperations()
        {
            var supplementOperation = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.AmountRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(30270), WorkItemAmount.Create(50), WorkItemUnit.Meter,
                new List<SupplementOperation> { supplementOperation }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(2740686.2m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenWorkItem_WithNegativeSupplementOperation_CalculateTotalOperationTime()
        {
            var amountRelatedSupplementOperation = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.AmountRelated(), SupplementOperationTime.Create(-8025), SupplementOperationAmount.Create(5));

            var unitRelatedSupplementOperation = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(-1025), SupplementOperationAmount.Create(2));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(20270), WorkItemAmount.Create(49), WorkItemUnit.Meter,
                new List<SupplementOperation> { amountRelatedSupplementOperation, unitRelatedSupplementOperation }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(1499308.552m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void IncludeUnitRelatedSupplementOperations()
        {
            var supplementOperation = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(30270), WorkItemAmount.Create(50), WorkItemUnit.Meter,
                new List<SupplementOperation> { supplementOperation }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(6628728.4m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void IncludeSupplementInCalculation()
        {
            var supplement = Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Empty(), SupplementNumber.Empty(), SupplementText.Empty(),
                SupplementPercentage.Create(25));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(57000), WorkItemAmount.Create(7), WorkItemUnit.Meter,
                new List<SupplementOperation>(), new List<Supplement> { supplement });

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(877002.0m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void IncludeMultipleSupplementsInCalculation()
        {
            var supplement1 = Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Guid.NewGuid()), SupplementNumber.Empty(),
                SupplementText.Empty(),
                SupplementPercentage.Create(25));
            var supplement2 = Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Create(Guid.NewGuid()), SupplementNumber.Empty(),
                SupplementText.Empty(),
                SupplementPercentage.Create(45));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(57000), WorkItemAmount.Create(7), WorkItemUnit.Meter,
                new List<SupplementOperation>(), new List<Supplement> { supplement1, supplement2 });

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(1192722.72m, result.TotalWorkTimeExpression.Evaluate().Value, 2);
        }

        [Fact]
        public void CalculateTotalPayment()
        {
            var amountRelatedOp = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.AmountRelated(), SupplementOperationTime.Create(9025), SupplementOperationAmount.Create(5));

            var unitRelatedOp = SupplementOperation.Create(Any.SupplementOperationId(), Any.CatalogSupplementOperationId(),
                Any.SupplementOperationText(),
                SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(1023), SupplementOperationAmount.Create(4));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(223200), WorkItemAmount.Create(3), WorkItemUnit.Meter,
                new List<SupplementOperation> { unitRelatedOp, amountRelatedOp }, new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(76.25764m, result.TotalPaymentExpression.Evaluate().Value, 5);
            Assert.Equal(1278358.5584m, result.TotalWorkTimeExpression.Evaluate().Value);
        }

        [Fact]
        public void CalculateTotalPaymentWithNonStanderBaseRates()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(323200), WorkItemAmount.Create(50), WorkItemUnit.Meter,
                new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplement = CreateBaseRateAndSupplement(60, 4, 8, 220, 2);
            var calc = new WorkItemCalculator(new BaseRateAndSupplementProxy(baseRateAndSupplement,
                Any.ProjectFolder(FolderRateAndSupplement.Create(baseRateAndSupplement))));

            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(1780.918186m, result.TotalPaymentExpression.Evaluate().Value, 5);
        }

        [Fact]
        public void CalculateTotalPaymentWithZeroSupplement()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(4500), WorkItemAmount.Create(2), WorkItemUnit.Meter,
                new List<SupplementOperation>(), new List<Supplement>());

            var baseRateAndSupplement = CreateBaseRateAndSupplement(0, 0, 0, 0, 0);
            var calc = new WorkItemCalculator(new BaseRateAndSupplementProxy(baseRateAndSupplement,
                Any.ProjectFolder(FolderRateAndSupplement.Create(baseRateAndSupplement))));

            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(9000m, result.TotalWorkTimeExpression.Evaluate().Value);
            Assert.Equal(0m, result.TotalPaymentExpression.Evaluate().Value);
        }

        [Fact]
        public void GivenOperationWorkItem_CalculateTotalOperationTime()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogOperationId(), Any.WorkItemOperationNumber(), Any.WorkItemDate(),
                Any.WorkItemUser(), Any.WorkItemText(), WorkItemDuration.Create(9900), WorkItemAmount.Create(14), new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(14.5382m, result.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(243714.24m, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenOperationWorkItem_WithNegativeOperationTime_CalculateTotalOperationTime()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogOperationId(), Any.WorkItemOperationNumber(), Any.WorkItemDate(),
                Any.WorkItemUser(), Any.WorkItemText(), WorkItemDuration.Create(-7200), WorkItemAmount.Create(2), new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(-1.5105m, result.TotalPaymentExpression.Evaluate().Value, 4);
            Assert.Equal(-25320.9600m, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenOperationWorkItem_WithSupplement_CalculateTotalOperationTime()
        {
            var supplement = Supplement.Create(SupplementId.Empty(), CatalogSupplementId.Empty(), SupplementNumber.Empty(), SupplementText.Empty(),
                SupplementPercentage.Create(55));

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogOperationId(), Any.WorkItemOperationNumber(), Any.WorkItemDate(),
                Any.WorkItemUser(), Any.WorkItemText(), WorkItemDuration.Create(9900), WorkItemAmount.Create(16),
                new List<Supplement> { supplement });

            var calc = GetDefaultWorkItemCalculator();
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(431722.368m, result.TotalWorkTimeExpression.Evaluate().Value, 4);
        }

        [Fact]
        public void GivenWorkItemWithFolderSupplements_ShouldCalculateCorrectly()
        {
            var folder = Any.ProjectFolder();

            folder.UpdateFolderSupplements(Any.ProjectId(),
                new List<FolderSupplement> { Any.FolderSupplementWithPercentage(100), Any.FolderSupplementWithPercentage(100) });

            var folderWork = Any.ProjectFolderWork();

            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogOperationId(), Any.WorkItemOperationNumber(), Any.WorkItemDate(),
                Any.WorkItemUser(), Any.WorkItemText(), WorkItemDuration.Create(9900), WorkItemAmount.Create(16),
                new List<Supplement>());

            folderWork.AddWorkItem(workItem, new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));

            var calc = new WorkItemCalculator(new BaseRateAndSupplementProxy(GetDefaultBaseRateAndSupplement(), folder));
            var result = calc.CalculateTotalOperationTime(workItem);

            Assert.Equal(49.85m, workItem.TotalPayment.Value, 2);
            Assert.Equal(49.85m, result.TotalPaymentExpression.Evaluate().Value, 2);
        }


        [Fact]
        public void OverflowWillThrowException()
        {
            var workItem = WorkItem.Create(Any.WorkItemId(), Any.CatalogMaterialId(), Any.WorkItemDate(), Any.WorkItemUser(), Any.WorkItemText(),
                Any.WorkItemEanNumber(),
                Any.WorkItemMountingCode(), WorkItemDuration.Create(281474976710656), WorkItemAmount.Create(281474976710656), WorkItemUnit.Meter,
                new Collection<SupplementOperation>(), new List<Supplement>());

            var calc = GetDefaultWorkItemCalculator();

            Assert.Throws<OverflowException>(() => calc.CalculateTotalOperationTime(workItem));
        }
    }
}
