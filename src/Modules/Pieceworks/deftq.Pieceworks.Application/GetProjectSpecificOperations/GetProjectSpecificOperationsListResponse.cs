namespace deftq.Pieceworks.Application.GetProjectSpecificOperations
{
    public class GetProjectSpecificOperationsListResponse
    {
        public IEnumerable<GetProjectSpecificOperationResponse> ProjectSpecificOperations { get; private set; }

        public GetProjectSpecificOperationsListResponse(IEnumerable<GetProjectSpecificOperationResponse> projectSpecificOperations)
        {
            ProjectSpecificOperations = projectSpecificOperations;
        }
    }

    public class GetProjectSpecificOperationResponse
    {
        public Guid ProjectSpecificOperationId { get; private set; }
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal OperationTimeMs { get; private set; }
        public decimal WorkingTimeMs { get; private set; }
        public decimal Payment { get; private set; }

        public GetProjectSpecificOperationResponse(Guid projectSpecificOperationId, string extraWorkAgreementNumber, string name, string description,
            decimal operationTimeMs, decimal workingTimeMs, decimal payment)
        {
            ProjectSpecificOperationId = projectSpecificOperationId;
            ExtraWorkAgreementNumber = extraWorkAgreementNumber;
            Name = name;
            Description = description;
            OperationTimeMs = operationTimeMs;
            WorkingTimeMs = workingTimeMs;
            Payment = payment;
        }
    }
}
