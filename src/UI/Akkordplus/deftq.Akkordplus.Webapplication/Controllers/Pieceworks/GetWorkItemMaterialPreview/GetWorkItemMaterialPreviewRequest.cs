namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemMaterialPreview
{
    public class GetWorkItemMaterialPreviewRequest
    {
        public Guid MaterialId { get; private set; }
        public decimal WorkItemAmount { get; private set; }
        public int WorkItemMountingCode { get; private set; }
        public IList<GetWorkItemMaterialPreviewSupplementOperationRequest> SupplementOperations { get; private set; }
        public IList<GetWorkItemMaterialPreviewSupplementRequest> Supplements { get; private set; }

        public GetWorkItemMaterialPreviewRequest(Guid materialId, decimal workItemAmount, int workItemMountingCode,
            IList<GetWorkItemMaterialPreviewSupplementOperationRequest> supplementOperations,
            IList<GetWorkItemMaterialPreviewSupplementRequest> supplements)
        {
            MaterialId = materialId;
            WorkItemAmount = workItemAmount;
            WorkItemMountingCode = workItemMountingCode;
            SupplementOperations = supplementOperations;
            Supplements = supplements;
        }
    }

    public class GetWorkItemMaterialPreviewSupplementOperationRequest
    {
        public Guid SupplementOperationId { get; private set; }
        public decimal Amount { get; private set; }

        public GetWorkItemMaterialPreviewSupplementOperationRequest(Guid supplementOperationId, decimal amount)
        {
            SupplementOperationId = supplementOperationId;
            Amount = amount;
        }
    }

    public class GetWorkItemMaterialPreviewSupplementRequest
    {
        public Guid SupplementId { get; private set; }

        public GetWorkItemMaterialPreviewSupplementRequest(Guid supplementId)
        {
            SupplementId = supplementId;
        }
    }
}
