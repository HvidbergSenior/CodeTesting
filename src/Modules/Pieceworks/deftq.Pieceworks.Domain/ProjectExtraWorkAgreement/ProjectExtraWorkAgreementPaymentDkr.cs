using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementPaymentDkr : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectExtraWorkAgreementPaymentDkr()
        {
            Value = decimal.Zero;
        }

        private ProjectExtraWorkAgreementPaymentDkr(decimal value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementPaymentDkr Create(decimal value)
        {
            return new ProjectExtraWorkAgreementPaymentDkr(value);
        }

        public static ProjectExtraWorkAgreementPaymentDkr Empty()
        {
            return new ProjectExtraWorkAgreementPaymentDkr(decimal.Zero);
        }
    }
}
