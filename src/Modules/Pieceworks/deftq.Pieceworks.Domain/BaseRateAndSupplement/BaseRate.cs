using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class BaseRate : ValueObject
    {
        public Decimal Value { get; private set; }

        private BaseRate()
        {
            Value = Decimal.Zero;
        }

        private BaseRate(decimal value)
        {
            Value = value;
        }

        public static BaseRate Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Base rate must be greater than or equal to 0");
            }
            return new BaseRate(value);
        }

        public static BaseRate Empty()
        {
            return new BaseRate(Decimal.Zero);
        }
    }
}
