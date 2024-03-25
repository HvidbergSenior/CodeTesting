using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class BaseRateRegulation : ValueObject

    {
        public Decimal Value { get; private set; }

        private BaseRateRegulation()
        {
            Value = Decimal.Zero;
        }

        private BaseRateRegulation(decimal value)
        {
            Value = value;
        }

        public static BaseRateRegulation Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Base rate regulation must be greater than or equal to 0");
            }
            return new BaseRateRegulation(value);
        }

        public static BaseRateRegulation Empty()
        {
            return new BaseRateRegulation(decimal.Zero);
        }
    }
}
