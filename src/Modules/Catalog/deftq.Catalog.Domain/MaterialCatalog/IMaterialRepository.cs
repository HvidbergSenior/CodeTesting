using deftq.BuildingBlocks.DataAccess;

namespace deftq.Catalog.Domain.MaterialCatalog
{
    public interface IMaterialRepository : IRepository<Material>, IReadonlyRepository<Material>
    {
        Task<Material> GetByEanNumber(string eanNumber, CancellationToken cancellationToken = default);
        
        Task<Material?> FindByEanNumber(string eanNumber, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<Material>> Search(string queryString, uint maxHits, CancellationToken cancellationToken);
        
        Task<DateTimeOffset> GetPublished();
    }
}
