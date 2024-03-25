using deftq.BuildingBlocks.DataAccess;
using deftq.BuildingBlocks.Exceptions;
using deftq.BuildingBlocks.Fakes;
using deftq.Catalog.Domain.MaterialCatalog;

namespace deftq.Catalog.Infrastructure
{
    public class MaterialInMemoryRepository : InMemoryRepository<Material>, IMaterialRepository
    {
        public Task<Material> GetByEanNumber(string eanNumber, CancellationToken cancellationToken = default)
        {
            var result = Query().FirstOrDefault(m => m.EanNumber.Value == eanNumber);
            if (result == null)
            {
                throw new NotFoundException($"Material with EAN number {eanNumber} not found");
            }
            return Task.FromResult(result);
        }

        public Task<Material?> FindByEanNumber(string eanNumber, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Query().FirstOrDefault(m => m.EanNumber.Value == eanNumber));
        }
        
        public async Task<IReadOnlyList<Material>> Search(string queryString, uint maxHits, CancellationToken cancellationToken)
        {
            return await Query().Where(m => m.Name.Value.Contains(queryString, StringComparison.OrdinalIgnoreCase) 
                                             || m.EanNumber.Value.EndsWith(queryString, StringComparison.OrdinalIgnoreCase))
                .Take((int)maxHits).ToListAsync(cancellationToken);
        }
        
        public Task<DateTimeOffset> GetPublished()
        {
            var materialPublished = Query().Select(m => m.Published.Value);

            DateTimeOffset result = DateTimeOffset.MinValue;
            if (materialPublished.Any())
            {
                result = materialPublished.Max();
            }
            
            return Task.FromResult(result);
        }
    }
}
