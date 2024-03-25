using deftq.BuildingBlocks.DataAccess;

namespace deftq.Catalog.Domain.OperationCatalog
{
    public interface IOperationRepository : IRepository<Operation>, IReadonlyRepository<Operation>
    {
        Task<IReadOnlyList<Operation>> Search(string queryString, uint maxHits, CancellationToken cancellationToken);
    }
}
