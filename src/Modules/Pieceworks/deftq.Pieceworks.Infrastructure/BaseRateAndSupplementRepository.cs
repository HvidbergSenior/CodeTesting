using deftq.Pieceworks.Domain;

namespace deftq.Pieceworks.Infrastructure
{
    public class BaseRateAndSupplementRepository : IBaseRateAndSupplementRepository
    {
        private BaseRateAndSupplement data;

        public BaseRateAndSupplementRepository()
        {
            var personalTimeSupplementIntervals = new List<PersonalTimeSupplementInterval>(){PersonalTimeSupplementInterval.Create(PersonalTimeSupplement.Create(6), DateInterval.Always())};
            var baseRateIntervals = new List<BaseRateInterval>(){BaseRateInterval.Create(BaseRate.Create(223.35m), DateInterval.Always() )};
            data = BaseRateAndSupplement.Create(IndirectTimeSupplement.Create(64), SiteSpecificTimeSupplement.Create(2), personalTimeSupplementIntervals, baseRateIntervals, BaseRateRegulation.Create(0));
        }

        public Task<BaseRateAndSupplement> Get(CancellationToken cancellationToken = default)
        {
            return GetById(Guid.Empty, cancellationToken);
        }
        
        public Task<BaseRateAndSupplement> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(data);
        }

        //To be implemented later
        public Task<BaseRateAndSupplement?> FindById(object id, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<BaseRateAndSupplement> Add(BaseRateAndSupplement entity, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<BaseRateAndSupplement> Update(BaseRateAndSupplement entity, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task Delete(BaseRateAndSupplement entity, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<bool> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task SaveChanges(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
