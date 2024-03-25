namespace deftq.Pieceworks.Application.GetWorkItems
{
    public class GetWorkItemsQueryResponse
    {
        public Guid ProjectId { get; private set; }
        public Guid ProjectFolderId { get; private set; }
        public IList<WorkItemResponse> WorkItems { get; private set; }

        private GetWorkItemsQueryResponse()
        {
            ProjectId = Guid.Empty;
            ProjectFolderId = Guid.Empty;
            WorkItems = new List<WorkItemResponse>();
        }

        public GetWorkItemsQueryResponse(Guid projectId, Guid projectFolderId, IList<WorkItemResponse> workItems)
        {
            ProjectId = projectId;
            ProjectFolderId = projectFolderId;
            WorkItems = workItems;
        }
    }

    public enum WorkItemType { Material, Operation }

    public class WorkItemResponse
    {
        public Guid WorkItemId { get; private set; }
        public string WorkItemText { get; private set; }
        public DateOnly WorkItemDate { get; private set; }
        public decimal WorkItemAmount { get; private set; }
        public decimal WorkItemOperationTimeMilliseconds { get; private set; }
        public decimal WorkItemTotalOperationTimeMilliseconds { get; private set; }
        public decimal WorkItemTotalPaymentDkr { get; private set; }
        public IList<WorkItemSupplementResponse> Supplements { get; private set; }

        public WorkItemType WorkItemType { get; private set; }

        public WorkItemMaterialResponse? WorkItemMaterial { get; private set; }
        public WorkItemOperationResponse? WorkItemOperation { get; private set; }

        private WorkItemResponse()
        {
            WorkItemId = Guid.NewGuid();
            WorkItemText = string.Empty;
            WorkItemDate = DateOnly.MinValue;
            WorkItemAmount = decimal.Zero;
            WorkItemOperationTimeMilliseconds = Decimal.Zero;
            WorkItemTotalOperationTimeMilliseconds = Decimal.Zero;
            Supplements = new List<WorkItemSupplementResponse>();
        }

        public WorkItemResponse(Guid workItemId, WorkItemType workItemType, string workItemText, DateOnly workItemDate, decimal workItemAmount,
            decimal workItemTotalOperationTimeMilliseconds, decimal workItemOperationTimeMilliseconds, decimal workItemTotalPaymentDkr,
            IList<WorkItemSupplementResponse> supplements, WorkItemMaterialResponse? workItemMaterial, WorkItemOperationResponse? workItemOperation)
        {
            WorkItemId = workItemId;
            WorkItemType = workItemType;
            WorkItemText = workItemText;
            WorkItemDate = workItemDate;
            WorkItemAmount = workItemAmount;
            WorkItemOperationTimeMilliseconds = workItemOperationTimeMilliseconds;
            WorkItemTotalOperationTimeMilliseconds = workItemTotalOperationTimeMilliseconds;
            WorkItemTotalPaymentDkr = workItemTotalPaymentDkr;
            Supplements = supplements;
            WorkItemMaterial = workItemMaterial;
            WorkItemOperation = workItemOperation;
        }
    }

    public sealed class WorkItemMaterialResponse
    {
        public string WorkItemEanNumber { get; private set; }
        public int WorkItemMountingCode { get; private set; }
        public string WorkItemMountingCodeText { get; private set; }
        public IList<WorkItemSupplementOperationResponse> SupplementOperations { get; private set; }

        private WorkItemMaterialResponse()
        {
            WorkItemEanNumber = string.Empty;
            WorkItemMountingCode = 0;
            WorkItemMountingCodeText = String.Empty;
            SupplementOperations = new List<WorkItemSupplementOperationResponse>();
        }

        public WorkItemMaterialResponse(string workItemEanNumber, int workItemMountingCode, string workItemMountingCodeText,
            IList<WorkItemSupplementOperationResponse> supplementOperations)
        {
            WorkItemEanNumber = workItemEanNumber;
            WorkItemMountingCode = workItemMountingCode;
            WorkItemMountingCodeText = workItemMountingCodeText;
            SupplementOperations = supplementOperations;
        }
    }

    public sealed class WorkItemOperationResponse
    {
        public string OperationNumber { get; private set; }

        private WorkItemOperationResponse()
        {
            OperationNumber = String.Empty;
        }

        public WorkItemOperationResponse(string operationNumber)
        {
            OperationNumber = operationNumber;
        }
    }

    public class WorkItemSupplementOperationResponse
    {
        public enum WorkItemSupplementOperationType { AmountRelated, UnitRelated }

        public Guid SupplementOperationId { get; private set; }
        public string Text { get; private set; }
        public WorkItemSupplementOperationType OperationType { get; private set; }
        public decimal OperationTimeMilliseconds { get; private set; }
        public decimal Amount { get; private set; }

        private WorkItemSupplementOperationResponse()
        {
            SupplementOperationId = Guid.Empty;
            Text = String.Empty;
            OperationType = WorkItemSupplementOperationType.AmountRelated;
            OperationTimeMilliseconds = 0;
            Amount = 0;
        }

        public WorkItemSupplementOperationResponse(Guid supplementOperationId, string text,
            WorkItemSupplementOperationType operationType, decimal operationTimeMilliseconds,
            decimal amount)
        {
            SupplementOperationId = supplementOperationId;
            Text = text;
            OperationType = operationType;
            OperationTimeMilliseconds = operationTimeMilliseconds;
            Amount = amount;
        }
    }

    public class WorkItemSupplementResponse
    {
        public Guid SupplementId { get; private set; }
        public string SupplementNumber { get; private set; }
        public string SupplementText { get; private set; }

        private WorkItemSupplementResponse()
        {
            SupplementId = Guid.NewGuid();
            SupplementNumber = string.Empty;
            SupplementText = String.Empty;
        }

        public WorkItemSupplementResponse(Guid supplementId, string supplementNumber, string supplementText)
        {
            SupplementId = supplementId;
            SupplementNumber = supplementNumber;
            SupplementText = supplementText;
        }
    }
}
