using deftq.BuildingBlocks.DataAccess;

namespace deftq.Catalog.Domain.SupplementCatalog
{
    public interface ISupplementRepository : IRepository<Supplement>, IReadonlyRepository<Supplement>
    {
        Task<IReadOnlyList<Supplement>> GetAllAsync(CancellationToken cancellationToken);
    }
}
