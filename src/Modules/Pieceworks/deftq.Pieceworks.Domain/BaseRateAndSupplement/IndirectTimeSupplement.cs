using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class IndirectTimeSupplement : ValueObject
    {
        public Decimal Value { get; private set; }

        private IndirectTimeSupplement()
        {
            Value = Decimal.Zero;
        }

        private IndirectTimeSupplement(decimal value)
        {
            Value = value;
        }

        public static IndirectTimeSupplement Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Indirect time supplement must be greater than or equal to 0");
            }
            return new IndirectTimeSupplement(value);
        }

        public static IndirectTimeSupplement Empty()
        {
            return new IndirectTimeSupplement(decimal.Zero);
        }
    }
}
