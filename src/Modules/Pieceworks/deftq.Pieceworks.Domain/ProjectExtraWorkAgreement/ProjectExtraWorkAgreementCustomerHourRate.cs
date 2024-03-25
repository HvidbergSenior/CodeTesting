using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectExtraWorkAgreement
{
    public sealed class ProjectExtraWorkCustomerHourRate : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectExtraWorkCustomerHourRate()
        {
            Value = decimal.Zero;
        }

        private ProjectExtraWorkCustomerHourRate(decimal value)
        {
            Value = value;
        }

        public static ProjectExtraWorkCustomerHourRate Create(decimal value)
        {
            return new ProjectExtraWorkCustomerHourRate(value);
        }

        public static ProjectExtraWorkCustomerHourRate Empty()
        {
            return new ProjectExtraWorkCustomerHourRate(decimal.Zero);
        }
    }
}
