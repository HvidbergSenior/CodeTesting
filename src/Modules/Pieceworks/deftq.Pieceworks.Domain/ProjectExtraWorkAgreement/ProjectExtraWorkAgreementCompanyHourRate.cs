using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementCompanyHourRate : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectExtraWorkAgreementCompanyHourRate()
        {
            Value = decimal.Zero;
        }

        private ProjectExtraWorkAgreementCompanyHourRate(decimal value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementCompanyHourRate Create(decimal value)
        {
            return new ProjectExtraWorkAgreementCompanyHourRate(value);
        }

        public static ProjectExtraWorkAgreementCompanyHourRate Empty()
        {
            return new ProjectExtraWorkAgreementCompanyHourRate(decimal.Zero);
        }
    }
}
