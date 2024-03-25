using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class SiteSpecificTimeSupplement : ValueObject
    {
        public Decimal Value { get; private set; }

        private SiteSpecificTimeSupplement()
        {
            Value = Decimal.Zero;
        }

        private SiteSpecificTimeSupplement(decimal value)
        {
            Value = value;
        }

        public static SiteSpecificTimeSupplement Create(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Site specific time supplement must be greater than or equal to 0");
            }
            return new SiteSpecificTimeSupplement(value);
        }

        public static SiteSpecificTimeSupplement Empty()
        {
            return new SiteSpecificTimeSupplement(Decimal.Zero);
        }
    }
}
