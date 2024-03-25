using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Fakes;
using deftq.Catalog.Domain.OperationCatalog;

namespace deftq.Catalog.Infrastructure
{
    public class OperationInMemoryRepository : InMemoryRepository<Operation>, IOperationRepository
    {
        public async Task<IReadOnlyList<Operation>> Search(string queryString, uint maxHits, CancellationToken cancellationToken)
        {
            return await Query().Where(o => o.OperationText.Value.Contains(queryString, StringComparison.Ordinal) 
                                            || o.OperationText.Value.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                .Take((int)maxHits).ToListAsync(cancellationToken);
        }
    }
}
