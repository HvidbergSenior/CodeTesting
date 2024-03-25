using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain;

namespace deftq.Pieceworks.Infrastructure
{
    public class BaseRateAndSupplementInMemoryRepository : InMemoryRepository<BaseRateAndSupplement>, IBaseRateAndSupplementRepository
    {
        private BaseRateAndSupplement data;

        public BaseRateAndSupplementInMemoryRepository()
        {
            var personalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>(){PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always())};
            var baseRateIntervals = new List<BaseRateInterval>(){BaseRateInterval.Create(BaseRate.Create(214.75m), DateInterval.Always() )};
            data = BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2), personalTimeSupplementIntervals, baseRateIntervals, BaseRateRegulation.Create(0));
        }
        
        public Task<BaseRateAndSupplement> Get(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(data);
        }
    }
}
