using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.BuildingBlocks.Exceptions;
using deftq.Catalog.Domain.MaterialCatalog;
using Marten;

namespace deftq.Catalog.Infrastructure
{
    public class MaterialRepository : MartenDocumentRepository<Material>, IMaterialRepository
    {
        private readonly IMartenConfig _martenConfig;

        public MaterialRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher, IMartenConfig martenConfig) :
            base(documentSession, aggregateEventsPublisher)
        {
            _martenConfig = martenConfig;
        }

        public async Task<IReadOnlyList<Material>> Search(string queryString, uint maxHits, CancellationToken cancellationToken)
        {
            return await Query().Where(m => m.Name.Value.WebStyleSearch(queryString, _martenConfig.FullTextSearchLanguage)
                                            || m.Name.Value.Contains(queryString, StringComparison.OrdinalIgnoreCase)
                                            || m.EanNumber.Value.EndsWith(queryString, StringComparison.OrdinalIgnoreCase))
                .Take((int)maxHits).ToListAsync(cancellationToken);
        }

        public Task<Material?> FindByEanNumber(string eanNumber, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Query().FirstOrDefault(m => m.EanNumber.Value == eanNumber));
        }

        public Task<Material> GetByEanNumber(string eanNumber, CancellationToken cancellationToken = default)
        {
            var result = Query().FirstOrDefault(m => m.EanNumber.Value == eanNumber);
            if (result == null)
            {
                throw new NotFoundException($"Material with EAN number {eanNumber} not found");
            }

            return Task.FromResult(result);
        }

        public async Task<DateTimeOffset> GetPublished()
        {
            var published = await Query().MaxAsync(m => m.Published.Value);
            return published;
        }
    }
}
