using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.projectCompensation
{
    public sealed class ProjectCompensationPayment : ValueObject
    {
        public decimal Value { get; private set; }

        private ProjectCompensationPayment()
        {
            Value = 0;
        }

        private ProjectCompensationPayment(decimal value)
        {
            Value = value;
        }

        public static ProjectCompensationPayment Create(decimal value)
        {
            if (value > 0)
            {
                return new ProjectCompensationPayment(value);
            }

            throw new ArgumentException("value can not be less than 0", nameof(value));
        }

        public static ProjectCompensationPayment Empty()
        {
            return new ProjectCompensationPayment(0);
        }
    }
}
