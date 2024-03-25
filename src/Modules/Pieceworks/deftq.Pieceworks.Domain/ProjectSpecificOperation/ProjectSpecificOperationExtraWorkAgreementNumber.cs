namespace deftq.Pieceworks.Domain.projectSpecificOperation
{
    public sealed class ProjectSpecificOperationExtraWorkAgreementNumber
    {
        public string Value { get; private set; }

        private ProjectSpecificOperationExtraWorkAgreementNumber()
        {
            Value = string.Empty;
        }

        private ProjectSpecificOperationExtraWorkAgreementNumber(string value)
        {
            Value = value;
        }

        public static ProjectSpecificOperationExtraWorkAgreementNumber Create(string value)
        {
            if (value.Length > 20)
            {
                throw new ArgumentException("Value cant be longer than 20 chars", nameof(value));
            }
            return new ProjectSpecificOperationExtraWorkAgreementNumber(value);
        }

        public static ProjectSpecificOperationExtraWorkAgreementNumber Empty()
        {
            return new ProjectSpecificOperationExtraWorkAgreementNumber(string.Empty);
        }
    }
}
