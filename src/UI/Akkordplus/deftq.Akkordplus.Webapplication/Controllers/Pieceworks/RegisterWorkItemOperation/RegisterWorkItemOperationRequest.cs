namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemOperation
{
    public class RegisterWorkItemOperationRequest
    {
        public Guid OperationId { get; private set; }
        public decimal WorkItemAmount { get; private set; }
        public IList<OperationSupplementRequest> Supplements { get; private set; }

        public RegisterWorkItemOperationRequest(Guid operationId, decimal workItemAmount, IList<OperationSupplementRequest> supplements)
        {
            OperationId = operationId;
            WorkItemAmount = workItemAmount;
            Supplements = supplements;
        }
    }
    
    public class OperationSupplementRequest
    {
        public Guid SupplementId { get; private set; }

        public OperationSupplementRequest(Guid supplementId)
        {
            SupplementId = supplementId;
        }
    }
}
