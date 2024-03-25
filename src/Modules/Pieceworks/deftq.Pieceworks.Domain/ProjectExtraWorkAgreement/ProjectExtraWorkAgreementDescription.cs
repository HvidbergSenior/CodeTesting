using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementDescription : ValueObject
    {
        public string Value { get; private set; }

        private ProjectExtraWorkAgreementDescription()
        {
            Value = string.Empty;
        }

        private ProjectExtraWorkAgreementDescription(string value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementDescription Create(string value)
        {
            return new ProjectExtraWorkAgreementDescription(value);
        }

        public static ProjectExtraWorkAgreementDescription Empty()
        {
            return new ProjectExtraWorkAgreementDescription(string.Empty);
        }
    }
}
