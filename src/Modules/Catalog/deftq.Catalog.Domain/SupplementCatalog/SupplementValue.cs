using deftq.BuildingBlocks.Domain;

namespace deftq.Catalog.Domain.SupplementCatalog
{
    public sealed class SupplementValue : ValueObject
    {
        public decimal Value { get; private set; }

        private SupplementValue()
        {
            Value = Decimal.Zero;
        }

        private SupplementValue(decimal value)
        {
            Value = value;
        }

        public static SupplementValue Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value),"Supplement value must be greater than or equal to 0");
            }
            return new SupplementValue(value);
        }
        
        public static SupplementValue Empty()
        {
            return new SupplementValue(decimal.Zero);
        }
    }
}
