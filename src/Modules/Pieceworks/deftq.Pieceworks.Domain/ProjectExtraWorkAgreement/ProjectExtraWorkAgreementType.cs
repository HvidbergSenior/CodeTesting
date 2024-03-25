using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementType : ValueObject
    {
        public string Value { get; private set; }

        private ProjectExtraWorkAgreementType()
        {
            Value = string.Empty;
        }

        private ProjectExtraWorkAgreementType(string value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementType Create(string value)
        {
            return new ProjectExtraWorkAgreementType(value);
        }

        public static ProjectExtraWorkAgreementType Empty()
        {
            return new ProjectExtraWorkAgreementType(String.Empty);
        }

        public static ProjectExtraWorkAgreementType CustomerHours()
        {
            return Create("customer hours");
        }

        public static ProjectExtraWorkAgreementType CompanyHours()
        {
            return Create("company hours");
        }

        public static ProjectExtraWorkAgreementType AgreedPayment()
        {
            return Create("agreed payment");
        }

        public static ProjectExtraWorkAgreementType Other()
        {
            return Create("other");
        }
    }
}
