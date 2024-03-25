namespace deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectSpecificOperation
{
    public class RegisterProjectSpecificOperationRequest
    {
        public string ExtraWorkAgreementNumber { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal OperationTimeMs { get; private set; }
        public decimal WorkingTimeMs { get; private set; }

        public RegisterProjectSpecificOperationRequest(string extraWorkAgreementNumber, string name, string? description, decimal operationTimeMs,
            decimal workingTimeMs)
        {
            ExtraWorkAgreementNumber = extraWorkAgreementNumber;
            Name = name;
            Description = description;
            OperationTimeMs = operationTimeMs;
            WorkingTimeMs = workingTimeMs;
        }
    }
}
