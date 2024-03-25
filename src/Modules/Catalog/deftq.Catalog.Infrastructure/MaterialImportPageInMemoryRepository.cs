using deftq.BuildingBlocks.Fakes;
using deftq.Catalog.Domain.MaterialImport;

namespace deftq.Catalog.Infrastructure
{
    public class MaterialImportPageInMemoryRepository : InMemoryRepository<MaterialImportPage>, IMaterialImportPageRepository
    {
        public Task<IList<CatalogPageInfo>> GetLatestCatalogPageInfo(CancellationToken cancellationToken)
        {
            IList<CatalogPageInfo> noCatalogReturnValue = new List<CatalogPageInfo>();

            var allPages = Query().ToList();

            if (allPages.Count == 0)
            {
                // No catalog pages found
                return Task.FromResult(noCatalogReturnValue);
            }

            var latestPublished = allPages.Select(p => p.Published).Max();

            var pagesFromLatestPublishedCatalog = allPages.Where(p => p.Published == latestPublished).ToList();

            IList<CatalogPageInfo> latestCatalogPageInfo = pagesFromLatestPublishedCatalog.Select(page =>
                new CatalogPageInfo(page.Published, page.StartRow, page.PageSize, page.IsLastPage, page.ContentSize)).ToList();
            
            return Task.FromResult(latestCatalogPageInfo);
        }
    }
}
