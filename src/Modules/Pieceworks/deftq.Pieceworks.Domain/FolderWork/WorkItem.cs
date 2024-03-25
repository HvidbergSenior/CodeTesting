using System.Collections.ObjectModel;
using deftq.BuildingBlocks.Domain;
using deftq.Pieceworks.Domain.Calculation;
using deftq.Pieceworks.Domain.FolderWork.Supplements;

namespace deftq.Pieceworks.Domain.FolderWork
{
    public sealed class WorkItem : Entity
    {
        public WorkItemId WorkItemId { get; private set; }
        public WorkItemDate Date { get; private set; }
        public WorkItemUser User { get; private set; }
        public WorkItemText Text { get; private set; }
        public WorkItemDuration OperationTime { get; private set; }
        public WorkItemDuration TotalWorkTime { get; private set; }
        public WorkItemPayment TotalPayment { get; private set; }
        public WorkItemAmount Amount { get; private set; }
        public IList<Supplement> Supplements { get; private set; }

        public WorkItemMaterial? WorkItemMaterial { get; private set; }
        public WorkItemOperation? WorkItemOperation { get; private set; }

        private WorkItem()
        {
            WorkItemId = WorkItemId.Empty();
            Id = WorkItemId.Value;
            Date = WorkItemDate.Empty();
            User = WorkItemUser.Empty();
            Text = WorkItemText.Empty();
            OperationTime = WorkItemDuration.Empty();
            TotalWorkTime = WorkItemDuration.Empty();
            TotalPayment = WorkItemPayment.Empty();
            Amount = WorkItemAmount.Empty();
            Supplements = new List<Supplement>();
        }

        private WorkItem(WorkItemId id, CatalogMaterialId catalogMaterialId, WorkItemDate date, WorkItemUser user, WorkItemText workItemText,
            WorkItemEanNumber eanNumber, WorkItemMountingCode mountingCode, WorkItemDuration operationTime,
            WorkItemDuration totalWorkTime, WorkItemPayment totalPayment, WorkItemAmount amount, WorkItemUnit unit,
            IList<SupplementOperation> supplementOperations, IList<Supplement> supplements)
        {
            WorkItemId = id;
            Id = WorkItemId.Value;
            Date = date;
            User = user;
            Text = workItemText;
            OperationTime = operationTime;
            TotalWorkTime = totalWorkTime;
            TotalPayment = totalPayment;
            Amount = amount;
            Supplements = supplements;

            WorkItemMaterial = WorkItemMaterial.Create(catalogMaterialId, eanNumber, mountingCode, unit, supplementOperations);
        }

        private WorkItem(WorkItemId id, CatalogOperationId catalogOperationId, WorkItemOperationNumber operationNumber, WorkItemDate date,
            WorkItemUser user, WorkItemText workItemText, WorkItemDuration operationTime, WorkItemDuration totalWorkTime,
            WorkItemPayment totalPayment, WorkItemAmount amount, IList<Supplement> supplements)
        {
            WorkItemId = id;
            Id = WorkItemId.Value;
            Date = date;
            User = user;
            Text = workItemText;
            OperationTime = operationTime;
            TotalWorkTime = totalWorkTime;
            TotalPayment = totalPayment;
            Amount = amount;
            Supplements = supplements;

            WorkItemOperation = WorkItemOperation.Create(catalogOperationId, operationNumber);
        }

        public static WorkItem Create(WorkItemId id, CatalogMaterialId catalogMaterialId, WorkItemDate date, WorkItemUser user, WorkItemText text,
            WorkItemEanNumber eanNumber, WorkItemMountingCode mountingCode, WorkItemDuration operationTime, WorkItemAmount amount,
            WorkItemUnit unit, IList<SupplementOperation> supplementOperations, IList<Supplement> supplements)
        {
            var workItem = new WorkItem(id, catalogMaterialId, date, user, text, eanNumber, mountingCode, operationTime,
                WorkItemDuration.Empty(), WorkItemPayment.Empty(), amount, unit, supplementOperations, supplements);
            workItem.AddDomainEvent(WorkItemCreatedDomainEvent.Create(id));
            return workItem;
        }

        public static WorkItem Create(WorkItemId id, CatalogOperationId catalogOperationId, WorkItemOperationNumber workItemOperationNumber,
            WorkItemDate date, WorkItemUser user, WorkItemText text, WorkItemDuration duration, WorkItemAmount amount,
            IList<Supplement> supplements)
        {
            var workItem = new WorkItem(id, catalogOperationId, workItemOperationNumber, date, user, text, duration,
                WorkItemDuration.Empty(), WorkItemPayment.Empty(), amount, supplements);
            workItem.AddDomainEvent(WorkItemCreatedDomainEvent.Create(id));
            return workItem;
        }

        public static WorkItem Empty()
        {
            return new WorkItem();
        }

        public void UpdateTotalOperationTime(TotalWorkTimeCalculationResult calculationResult)
        {
            TotalWorkTime = WorkItemDuration.Create(calculationResult.TotalWorkTimeExpression.Evaluate().Value);
            TotalPayment = WorkItemPayment.Create(calculationResult.TotalPaymentExpression.Evaluate().Value);
        }

        public bool IsMaterial()
        {
            return WorkItemMaterial is not null;
        }
        
        public bool HasUnit()
        {
            return IsMaterial() && WorkItemMaterial?.Unit is not null;
        }

        public bool IsOperation()
        {
            return WorkItemOperation is not null;
        }

        public void UpdateAmount(WorkItemAmount workItemAmount, BaseRateAndSupplementProxy baseRateAndSupplementProxy)
        {
            if (Amount == workItemAmount)
            {
                return;
            }
            Amount = workItemAmount;
            var workItemCalculator = new WorkItemCalculator(baseRateAndSupplementProxy);
            var calculationResult = workItemCalculator.CalculateTotalOperationTime(this);
            this.UpdateTotalOperationTime(calculationResult);
        }
    }

    public sealed class WorkItemMaterial
    {
        public CatalogMaterialId CatalogMaterialId { get; private set; }
        public WorkItemEanNumber EanNumber { get; private set; }
        public WorkItemMountingCode MountingCode { get; private set; }
        public WorkItemUnit Unit { get; private set; }
        public IList<SupplementOperation> SupplementOperations { get; private set; }

        private WorkItemMaterial()
        {
            CatalogMaterialId = CatalogMaterialId.Empty();
            EanNumber = WorkItemEanNumber.Empty();
            MountingCode = WorkItemMountingCode.FromCode(3);
            Unit = WorkItemUnit.Empty();
            SupplementOperations = new Collection<SupplementOperation>();
        }

        private WorkItemMaterial(CatalogMaterialId catalogMaterialId, WorkItemEanNumber eanNumber, WorkItemMountingCode mountingCode,
            WorkItemUnit unit, IList<SupplementOperation> supplementOperations)
        {
            CatalogMaterialId = catalogMaterialId;
            EanNumber = eanNumber;
            MountingCode = mountingCode;
            Unit = unit;
            SupplementOperations = supplementOperations;
        }

        public static WorkItemMaterial Create(CatalogMaterialId catalogMaterialId, WorkItemEanNumber eanNumber, WorkItemMountingCode mountingCode,
            WorkItemUnit unit, IList<SupplementOperation> supplementOperations)
        {
            return new WorkItemMaterial(catalogMaterialId, eanNumber, mountingCode, unit, supplementOperations);
        }
    }

    public sealed class WorkItemOperation
    {
        public CatalogOperationId CatalogOperationId { get; private set; }
        public WorkItemOperationNumber WorkItemOperationNumber { get; private set; }

        private WorkItemOperation()
        {
            CatalogOperationId = CatalogOperationId.Empty();
            WorkItemOperationNumber = WorkItemOperationNumber.Empty();
        }

        private WorkItemOperation(CatalogOperationId catalogOperationId, WorkItemOperationNumber workItemOperationNumber)
        {
            CatalogOperationId = catalogOperationId;
            WorkItemOperationNumber = workItemOperationNumber;
        }

        public static WorkItemOperation Create(CatalogOperationId catalogOperationId, WorkItemOperationNumber workItemOperationNumber)
        {
            return new WorkItemOperation(catalogOperationId, workItemOperationNumber);
        }
    }
}
