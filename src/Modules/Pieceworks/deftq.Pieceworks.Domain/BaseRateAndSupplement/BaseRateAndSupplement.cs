using deftq.BuildingBlocks.Domain;

namespace deftq.Pieceworks.Domain
{
    public sealed class BaseRateAndSupplement : Entity
    {
        public IndirectTimeSupplement IndirectTimeSupplement { get; private set; }
        public SiteSpecificTimeSupplement SiteSpecificTimeSupplement { get; private set; }
        public IList<PersonalTimeSupplementInterval> PersonalTimeSupplementIntervals { get; private set; }
        public IList<BaseRateInterval> BaseRateIntervals { get; private set; }
        public BaseRateRegulation BaseRateRegulation { get; private set; }

        private BaseRateAndSupplement()
        {
            IndirectTimeSupplement = IndirectTimeSupplement.Empty();
            SiteSpecificTimeSupplement = SiteSpecificTimeSupplement.Empty();
            PersonalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>();
            BaseRateIntervals = new List<BaseRateInterval>();
            BaseRateRegulation = BaseRateRegulation.Empty();
        }

        private BaseRateAndSupplement(IndirectTimeSupplement indirectTimeSupplement, SiteSpecificTimeSupplement siteSpecificTimeSupplement, IList<PersonalTimeSupplementInterval> personalTimeSupplementIntervals, IList<BaseRateInterval> baseRateIntervals, BaseRateRegulation baseRateRegulation)
        {
            IndirectTimeSupplement = indirectTimeSupplement;
            SiteSpecificTimeSupplement = siteSpecificTimeSupplement;
            PersonalTimeSupplementIntervals = personalTimeSupplementIntervals;
            BaseRateIntervals = baseRateIntervals;
            BaseRateRegulation = baseRateRegulation;
        }

        public static BaseRateAndSupplement Create(IndirectTimeSupplement indirectTimeSupplement, SiteSpecificTimeSupplement siteSpecificTimeSupplement, IList<PersonalTimeSupplementInterval> personalTimeSupplementInterval, IList<BaseRateInterval> baseRateInterval, BaseRateRegulation baseRateRegulation)
        {
            var baseRateAndSupplement = new BaseRateAndSupplement(indirectTimeSupplement, siteSpecificTimeSupplement, personalTimeSupplementInterval, baseRateInterval, baseRateRegulation);
            return baseRateAndSupplement;
        }
        public BaseRateInterval GetBaseRateInterval(DateOnly day)
        {
            return BaseRateIntervals.First(d => d.IsApplicable(day));
        }

        public PersonalTimeSupplementInterval GetPersonalTimeSupplementInterval(DateOnly day)
        {
            return PersonalTimeSupplementIntervals.First(d => d.IsApplicable(day));
        }
    }
}
