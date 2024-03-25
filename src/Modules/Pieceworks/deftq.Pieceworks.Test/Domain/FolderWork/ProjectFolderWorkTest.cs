using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.FolderWork;
using deftq.Pieceworks.Domain.FolderWork.Supplements;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;
using static deftq.Pieceworks.Test.Domain.Calculation.BaseRateAndSupplementTestUtil;

namespace deftq.Pieceworks.Test.Domain.FolderWork
{
    public class ProjectFolderWorkTest
    {
        private readonly ITestOutputHelper output;

        public ProjectFolderWorkTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void When_WorkItemAddedToFolder_FolderShouldContainWorkItem()
        {
            var folderWork = Any.ProjectFolderWork();

            var workItem = Any.WorkItem();
            folderWork.AddWorkItem(workItem, GetDefaultBaseRateAndSupplementProxy());

            Assert.Single(folderWork.WorkItems);
            Assert.Equal(workItem, folderWork.WorkItems[0]);
        }

        [Fact]
        public void GivenWorkItems_WhenCopyingUnknownId_ShouldThrowException()
        {
            var sourceFolderWork = Any.ProjectFolderWork();
            var destinationFolderWork = Any.ProjectFolderWork();
            var executionContext = new FakeExecutionContext();
            var baseRateAndSupplement = Any.BaseRateAndSupplement();

            Assert.Throws<WorkItemNotFoundException>(() =>
                sourceFolderWork.CopyWorkItems(destinationFolderWork, GetDefaultBaseRateAndSupplementProxy(), new List<WorkItemId> { Any.WorkItemId() },
                    executionContext));
        }

        [Fact]
#pragma warning disable MA0051
        public void GivenWorkItems_WhenCopying_WorkItemsShouldBeCopiedToDestination()
#pragma warning restore MA0051
        {
            AssertionOptions.FormattingOptions.MaxLines = Int32.MaxValue;

            var sourceFolderWork = Any.ProjectFolderWork();
            var sourceWorkItemMaterial = CreateSourceWorkItemMaterial();
            var sourceWorkItemOperation = CreateSourceWorkItemOperation();
            sourceFolderWork.AddWorkItem(sourceWorkItemMaterial, GetDefaultBaseRateAndSupplementProxy());
            sourceFolderWork.AddWorkItem(sourceWorkItemOperation, GetDefaultBaseRateAndSupplementProxy());
            var destinationFolderWork = Any.ProjectFolderWork();
            var executionContext = new FakeExecutionContext(Guid.NewGuid(), "poul@ofir.dk");

            sourceFolderWork.CopyWorkItems(destinationFolderWork, GetDefaultBaseRateAndSupplementProxy(),
                new List<WorkItemId> { sourceWorkItemMaterial.WorkItemId, sourceWorkItemOperation.WorkItemId }, executionContext);

            destinationFolderWork.WorkItems.Should().HaveCount(2);
            var materialCopy = destinationFolderWork.WorkItems.First(w => w.IsMaterial());
            var operationCopy = destinationFolderWork.WorkItems.First(w => w.IsOperation());

            materialCopy.Amount.Should().Be(sourceWorkItemMaterial.Amount);
            materialCopy.Date.Value.Should().Be(sourceWorkItemMaterial.Date.Value.AddDays(1));
            materialCopy.Text.Should().Be(sourceWorkItemMaterial.Text);
            materialCopy.TotalPayment.Should().Be(sourceWorkItemMaterial.TotalPayment);
            materialCopy.TotalWorkTime.Should().Be(sourceWorkItemMaterial.TotalWorkTime);
            materialCopy.User.UserId.Should().Be(executionContext.UserId);
            materialCopy.User.UserName.Should().Be(executionContext.UserName);
            materialCopy.Supplements[0].SupplementNumber.Should().Be(sourceWorkItemMaterial.Supplements[0].SupplementNumber);
            materialCopy.Supplements[0].SupplementPercentage.Should().Be(sourceWorkItemMaterial.Supplements[0].SupplementPercentage);
            materialCopy.Supplements[0].SupplementText.Should().Be(sourceWorkItemMaterial.Supplements[0].SupplementText);
            materialCopy.Supplements[0].CatalogSupplementId.Should().Be(sourceWorkItemMaterial.Supplements[0].CatalogSupplementId);
            materialCopy.WorkItemMaterial!.CatalogMaterialId.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.CatalogMaterialId);
            materialCopy.WorkItemMaterial!.EanNumber.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.EanNumber);
            materialCopy.WorkItemMaterial!.MountingCode.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.MountingCode);
            materialCopy.WorkItemMaterial!.Unit.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.Unit);
            materialCopy.WorkItemMaterial!.SupplementOperations[0].Text.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.SupplementOperations[0].Text);
            materialCopy.WorkItemMaterial!.SupplementOperations[0].OperationAmount.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.SupplementOperations[0].OperationAmount);
            materialCopy.WorkItemMaterial!.SupplementOperations[0].OperationTime.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.SupplementOperations[0].OperationTime);
            materialCopy.WorkItemMaterial!.SupplementOperations[0].OperationType.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.SupplementOperations[0].OperationType);
            materialCopy.WorkItemMaterial!.SupplementOperations[0].CatalogSupplementOperationId.Should().Be(sourceWorkItemMaterial.WorkItemMaterial!.SupplementOperations[0].CatalogSupplementOperationId);

            operationCopy.Amount.Should().Be(sourceWorkItemOperation.Amount);
            operationCopy.Date.Value.Should().Be(sourceWorkItemOperation.Date.Value.AddDays(1));
            operationCopy.Text.Should().Be(sourceWorkItemOperation.Text);
            operationCopy.TotalPayment.Should().Be(sourceWorkItemOperation.TotalPayment);
            operationCopy.TotalWorkTime.Should().Be(sourceWorkItemOperation.TotalWorkTime);
            operationCopy.User.UserId.Should().Be(executionContext.UserId);
            operationCopy.User.UserName.Should().Be(executionContext.UserName);
            operationCopy.Supplements[0].SupplementNumber.Should().Be(sourceWorkItemOperation.Supplements[0].SupplementNumber);
            operationCopy.Supplements[0].SupplementPercentage.Should().Be(sourceWorkItemOperation.Supplements[0].SupplementPercentage);
            operationCopy.Supplements[0].SupplementText.Should().Be(sourceWorkItemOperation.Supplements[0].SupplementText);
            operationCopy.Supplements[0].CatalogSupplementId.Should().Be(sourceWorkItemOperation.Supplements[0].CatalogSupplementId);
            operationCopy.WorkItemOperation!.CatalogOperationId.Should().Be(sourceWorkItemOperation.WorkItemOperation!.CatalogOperationId);
            operationCopy.WorkItemOperation!.WorkItemOperationNumber.Should().Be(sourceWorkItemOperation.WorkItemOperation!.WorkItemOperationNumber);
            
            destinationFolderWork.DomainEvents.Should().ContainSingle().Which.Should().BeOfType<WorkItemsCopiedDomainEvent>();
        }

        private static WorkItem CreateSourceWorkItemMaterial()
        {
            var workItemId = WorkItemId.Create(Guid.NewGuid());
            var catalogMaterialId = CatalogMaterialId.Create(Guid.NewGuid());
            var workItemDate = WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Today).AddDays(-1));
            var workItemUser = WorkItemUser.Create(Guid.NewGuid(), "hans@hansen.dk");
            var workItemText = WorkItemText.Create("");
            var workItemEanNumber = WorkItemEanNumber.Create("1234567890123");
            var workItemMountingCode = WorkItemMountingCode.FromCode(3);
            var workItemDuration = WorkItemDuration.Create(123);
            var workItemAmount = WorkItemAmount.Create(7);
            var workItemUnit = WorkItemUnit.Piece;
            var supplementOperations = new List<SupplementOperation>
            {
                SupplementOperation.Create(SupplementOperationId.Create(Guid.NewGuid()), CatalogSupplementOperationId.Create(Guid.NewGuid()),
                    SupplementOperationText.Create("Tilslutte 5 ledere"), SupplementOperationType.UnitRelated(), SupplementOperationTime.Create(888),
                    SupplementOperationAmount.Create(15))
            };
            var supplements = new List<Supplement>
            {
                Supplement.Create(SupplementId.Create(Guid.NewGuid()), CatalogSupplementId.Create(Guid.NewGuid()), SupplementNumber.Create("123"),
                    SupplementText.Create("bah25"), SupplementPercentage.Create(10))
            };
            var workItem = WorkItem.Create(workItemId, catalogMaterialId, workItemDate, workItemUser, workItemText, workItemEanNumber,
                workItemMountingCode, workItemDuration, workItemAmount, workItemUnit, supplementOperations, supplements);
            return workItem;
        }

        private static WorkItem CreateSourceWorkItemOperation()
        {
            var workItemId = WorkItemId.Create(Guid.NewGuid());
            var catalogOperationId = CatalogOperationId.Create(Guid.NewGuid());
            var workItemDate = WorkItemDate.Create(DateOnly.FromDateTime(DateTime.Today).AddDays(-1));
            var workItemUser = WorkItemUser.Create(Guid.NewGuid(), "Teis@hansen.dk");
            var workItemText = WorkItemText.Create("Byggestrøm");
            var workItemDuration = WorkItemDuration.Create(1234);
            var workItemAmount = WorkItemAmount.Create(74);
            var supplements = new List<Supplement>
            {
                Supplement.Create(SupplementId.Create(Guid.NewGuid()), CatalogSupplementId.Create(Guid.NewGuid()), SupplementNumber.Create("123"),
                    SupplementText.Create("bah25"), SupplementPercentage.Create(10))
            };
            var workItemOperationNumber = WorkItemOperationNumber.Create("667");
            var workItem = WorkItem.Create(workItemId, catalogOperationId, workItemOperationNumber, workItemDate, workItemUser, workItemText,
                workItemDuration, workItemAmount, supplements);
            return workItem;
        }
    }
}
