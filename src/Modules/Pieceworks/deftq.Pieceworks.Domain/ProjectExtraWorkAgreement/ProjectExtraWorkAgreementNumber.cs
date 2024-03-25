using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementNumber : ValueObject
    {
        public string Value { get; private set; }

        private ProjectExtraWorkAgreementNumber()
        {
            Value = string.Empty;
        }

        private ProjectExtraWorkAgreementNumber(string value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementNumber Create(string value)
        {
            if (value.Length > 20)
            {
                throw new ArgumentException("Value cant be longer than 20 chars", nameof(value));
            }
            return new ProjectExtraWorkAgreementNumber(value);
        }

        public static ProjectExtraWorkAgreementNumber Empty()
        {
            return new ProjectExtraWorkAgreementNumber(string.Empty);
        }
    }
}
