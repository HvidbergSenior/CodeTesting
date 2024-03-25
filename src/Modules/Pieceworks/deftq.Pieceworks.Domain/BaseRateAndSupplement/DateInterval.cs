using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class DateInterval : ValueObject
    {
        public DateOnly StartDate { get; private set; }
        public DateOnly EndDate { get; private set; }
        

        private DateInterval()
        {
            StartDate = DateOnly.MinValue;
            EndDate = DateOnly.MaxValue;
        }

        private DateInterval(DateOnly startDate, DateOnly endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public static DateInterval Create(DateOnly startDate, DateOnly endDate)
        {
            return new DateInterval(startDate, endDate);
        }

        public static DateInterval Empty()
        {
            return new DateInterval(DateOnly.MinValue, DateOnly.MinValue);
        }

        public static DateInterval Always()
        {
            return new DateInterval(DateOnly.MinValue, DateOnly.MaxValue);
        }

        public bool IsDateIncluded(DateOnly date)
        {
            return date >= StartDate && date <= EndDate;
        }
    }
}
