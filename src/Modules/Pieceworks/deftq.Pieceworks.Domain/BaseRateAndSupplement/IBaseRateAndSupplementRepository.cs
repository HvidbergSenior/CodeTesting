using deftq.BuildingBlocks.DataAccess;

namespace deftq.Pieceworks.Domain
{
    public interface IBaseRateAndSupplementRepository : IRepository<BaseRateAndSupplement>
    {
        public Task<BaseRateAndSupplement> Get(CancellationToken cancellationToken = default);
    }
}
