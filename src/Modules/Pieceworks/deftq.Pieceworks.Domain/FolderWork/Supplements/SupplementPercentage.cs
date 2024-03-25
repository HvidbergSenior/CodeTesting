using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementPercentage : ValueObject
    {
        public decimal Value { get; private set; }

        private SupplementPercentage()
        {
            Value = decimal.Zero;
        }

        private SupplementPercentage(decimal value)
        {
            Value = value;
        }

        public static SupplementPercentage Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Supplement percentage must be greater than or equal to 0");
            }
            return new SupplementPercentage(value);
        }

        public static SupplementPercentage Empty()
        {
            return new SupplementPercentage(Decimal.Zero);
        }
    }
}
