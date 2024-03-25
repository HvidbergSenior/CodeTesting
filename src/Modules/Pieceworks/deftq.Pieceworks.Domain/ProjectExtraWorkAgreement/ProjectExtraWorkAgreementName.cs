using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementName : ValueObject
    {
        public string Value { get; private set; }

        private ProjectExtraWorkAgreementName()
        {
            Value = string.Empty;
        }

        private ProjectExtraWorkAgreementName(string value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementName Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Value is empty", nameof(value));
            }
            
            if (value.Length > 40)
            {
                throw new ArgumentException("Value cant be longer than 40 chars", nameof(value));
            }
            return new ProjectExtraWorkAgreementName(value);
        }

        public static ProjectExtraWorkAgreementName Empty()
        {
            return new ProjectExtraWorkAgreementName(string.Empty);
        }
    }
}
