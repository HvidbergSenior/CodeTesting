namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterWorkItemMaterial
{
    public class RegisterWorkItemMaterialRequest
    {
        public Guid MaterialId { get; private set; }
        public decimal WorkItemAmount { get; private set; }
        public int WorkItemMountingCode { get; private set; }
        public IList<MaterialSupplementOperationRequest> SupplementOperations { get; private set; }
        public IList<MaterialSupplementRequest> Supplements { get; private set; }

        public RegisterWorkItemMaterialRequest(Guid materialId, decimal workItemAmount, int workItemMountingCode, IList<MaterialSupplementOperationRequest> supplementOperations, IList<MaterialSupplementRequest> supplements)
        {
            MaterialId = materialId;
            WorkItemAmount = workItemAmount;
            WorkItemMountingCode = workItemMountingCode;
            SupplementOperations = supplementOperations;
            Supplements = supplements;
        }
    }

    public class MaterialSupplementOperationRequest
    {
        public Guid SupplementOperationId { get; private set; }
        public decimal Amount { get; private set; }

        public MaterialSupplementOperationRequest(Guid supplementOperationId, decimal amount)
        {
            SupplementOperationId = supplementOperationId;
            Amount = amount;
        }
    }
    
    public class MaterialSupplementRequest
    {
        public Guid SupplementId { get; private set; }

        public MaterialSupplementRequest(Guid supplementId)
        {
            SupplementId = supplementId;
        }
    }
}
