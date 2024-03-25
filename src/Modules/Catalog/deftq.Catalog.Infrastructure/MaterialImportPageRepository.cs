using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.BuildingBlocks.Events;
using deftq.Catalog.Domain.MaterialImport;
using Marten;
using Marten.Linq.MatchesSql;

namespace deftq.Catalog.Infrastructure
{
    public class MaterialImportPageRepository : MartenDocumentRepository<MaterialImportPage>, IMaterialImportPageRepository
    {
        private readonly IDocumentSession _documentSession;

        public MaterialImportPageRepository(IDocumentSession documentSession, IEntityEventsPublisher aggregateEventsPublisher) : base(documentSession,
            aggregateEventsPublisher)
        {
            _documentSession = documentSession;
        }
        
        public Task<IList<CatalogPageInfo>> GetLatestCatalogPageInfo(CancellationToken cancellationToken)
        {
            var whereClause = @"CAST(data->>'Published' as timestamptz) =
                (SELECT 
                CAST(data->>'Published' AS timestamptz) as published
                FROM mt_doc_materialimportpage
                ORDER BY published DESC
                LIMIT 1)";

            var latestCatalogPages = _documentSession.Query<MaterialImportPage>().Where(x => x.MatchesSql(whereClause));
            IList<CatalogPageInfo> latestCatalogPageInfo = latestCatalogPages.Select(info =>
                new CatalogPageInfo(info.Published, info.StartRow, info.PageSize, info.IsLastPage, info.ContentSize)).ToList();
            return Task.FromResult(latestCatalogPageInfo);
        }
    }
}
