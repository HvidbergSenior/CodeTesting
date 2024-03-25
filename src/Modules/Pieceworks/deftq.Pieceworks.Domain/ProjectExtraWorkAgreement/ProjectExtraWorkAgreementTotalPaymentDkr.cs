using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkAgreementTotalPaymentDkr : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectExtraWorkAgreementTotalPaymentDkr()
        {
            Value = decimal.Zero;
        }

        private ProjectExtraWorkAgreementTotalPaymentDkr(decimal value)
        {
            Value = value;
        }

        public static ProjectExtraWorkAgreementTotalPaymentDkr Create(decimal value)
        {
            return new ProjectExtraWorkAgreementTotalPaymentDkr(value);
        }

        public static ProjectExtraWorkAgreementTotalPaymentDkr Empty()
        {
            return new ProjectExtraWorkAgreementTotalPaymentDkr(decimal.Zero);
        }
    }
}
