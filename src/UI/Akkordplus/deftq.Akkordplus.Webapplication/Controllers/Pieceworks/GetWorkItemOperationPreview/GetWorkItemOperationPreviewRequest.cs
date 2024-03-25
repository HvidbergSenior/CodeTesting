namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.GetWorkItemOperationPreview
{
    public class GetWorkItemOperationPreviewRequest
    {
        public Guid OperationId { get; private set; }
        public decimal WorkItemAmount { get; private set; }
        public IList<GetWorkItemOperationPreviewSupplementRequest> Supplements { get; private set; }

        public GetWorkItemOperationPreviewRequest(Guid operationId, decimal workItemAmount,
            IList<GetWorkItemOperationPreviewSupplementRequest> supplements)
        {
            OperationId = operationId;
            WorkItemAmount = workItemAmount;
            Supplements = supplements;
        }
    }
    
    public class GetWorkItemOperationPreviewSupplementRequest
    {
        public Guid SupplementId { get; private set; }

        public GetWorkItemOperationPreviewSupplementRequest(Guid supplementId)
        {
            SupplementId = supplementId;
        }
    }
}
