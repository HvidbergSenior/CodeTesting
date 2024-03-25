using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class PersonalTimeSupplementInterval : Entity
    {
        public PersonalTimeSupplement PersonalTimeSupplement { get; private set; }
        public DateInterval DateInterval { get; private set; }

        private PersonalTimeSupplementInterval()
        {
            Id = Guid.NewGuid();
            PersonalTimeSupplement = PersonalTimeSupplement.Empty();
            DateInterval = DateInterval.Empty();
        }

        private PersonalTimeSupplementInterval(PersonalTimeSupplement personalTimeSupplement, DateInterval dateInterval)
        {
            Id = Guid.NewGuid();
            PersonalTimeSupplement = personalTimeSupplement;
            DateInterval = dateInterval;
        }

        public static PersonalTimeSupplementInterval Create(PersonalTimeSupplement personalTimeSupplement, DateInterval dateInterval)
        {
            return new PersonalTimeSupplementInterval(personalTimeSupplement, dateInterval);
        }
        
        public bool IsApplicable(DateOnly date)
        {
            return DateInterval.IsDateIncluded(date);
        }
    }
}
