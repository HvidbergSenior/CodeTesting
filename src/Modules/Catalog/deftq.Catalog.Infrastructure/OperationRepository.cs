using Baseline;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Catalog.Domain.OperationCatalog;
using Marten;

namespace deftq.Catalog.Infrastructure
{
    public class OperationRepository : MartenDocumentRepository<Operation>, IOperationRepository
    {
        private readonly IMartenConfig _martenConfig;

        public OperationRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher, IMartenConfig martenConfig) : base(documentSession, aggregateEventsPublisher)
        {
            _martenConfig = martenConfig;
        }

        public async Task<IReadOnlyList<Operation>> Search(string queryString, uint maxHits, CancellationToken cancellationToken)
        {
            return await Query().Where(o => o.OperationText.Value.WebStyleSearch(queryString, _martenConfig.FullTextSearchLanguage) 
                                            || o.OperationText.Value.Contains(queryString, StringComparison.OrdinalIgnoreCase)
                                            || o.OperationNumber.Value.Contains(queryString, StringComparison.OrdinalIgnoreCase))
                .Take((int)maxHits).ToListAsync(cancellationToken);
        }
    }
}
