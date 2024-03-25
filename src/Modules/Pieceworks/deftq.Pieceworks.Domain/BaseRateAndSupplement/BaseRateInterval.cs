using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class BaseRateInterval : Entity
    {
        public BaseRate BaseRate { get; private set; }
        public DateInterval DateInterval { get; private set; }

        private BaseRateInterval()
        {
            Id = Guid.NewGuid();
            BaseRate = BaseRate.Empty();
            DateInterval = DateInterval.Empty();
        }

        private BaseRateInterval(BaseRate baseRate, DateInterval interval)
        {
            Id = Guid.NewGuid(); 
            BaseRate = baseRate;
            DateInterval = interval;
        }

        public static BaseRateInterval Create(BaseRate baseRate, DateInterval interval)
        {
            return new BaseRateInterval(baseRate, interval);
        }

        public bool IsApplicable(DateOnly date)
        {
            return DateInterval.IsDateIncluded(date);
        }
    }
}
