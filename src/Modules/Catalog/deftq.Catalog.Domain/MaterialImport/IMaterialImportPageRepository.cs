using deftq.BuildingBlocks.DataAccess;

namespace deftq.Catalog.Domain.MaterialImport
{
    public interface IMaterialImportPageRepository : IRepository<MaterialImportPage>, IReadonlyRepository<MaterialImportPage>
    {
        Task<IList<CatalogPageInfo>> GetLatestCatalogPageInfo(CancellationToken cancellationToken);
    }
    
    public class CatalogPageInfo
    {
        public DateTimeOffset Published { get; private set; }
        public int StartRow { get; private set; }
        public int PageSize { get; private set; }
        public bool IsLastPage { get; private set; }
        public int ContentSize { get; private set; }

        public CatalogPageInfo(DateTimeOffset published, int startRow, int pageSize, bool isLastPage, int contentSize)
        {
            Published = published;
            StartRow = startRow;
            PageSize = pageSize;
            IsLastPage = isLastPage;
            ContentSize = contentSize;
        }
    }
}
