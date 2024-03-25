using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain.FolderWork.Supplements
{
    public sealed class SupplementOperationAmount : ValueObject
    {
        public decimal Value { get; private set; }

        private SupplementOperationAmount()
        {
            Value = decimal.Zero;
        }

        private SupplementOperationAmount(decimal value)
        {
            Value = value;
        }

        public static SupplementOperationAmount Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Operation amount must be greater than or equal to 0");
            }
            return new SupplementOperationAmount(value);
        }

        public static SupplementOperationAmount Empty()
        {
            return new SupplementOperationAmount(Decimal.Zero);
        }
    }
}
