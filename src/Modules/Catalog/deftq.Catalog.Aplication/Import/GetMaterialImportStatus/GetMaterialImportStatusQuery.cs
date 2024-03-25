using deftq.BuildingBlocks.Application.Queries;
using deftq.BuildingBlocks.DataAccess;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.MaterialImport;

namespace deftq.Catalog.Application.Import.GetMaterialImportStatus
{
    public sealed class GetMaterialImportStatusQuery : IQuery<GetMaterialImportStatusQueryResponse>
    {
        private GetMaterialImportStatusQuery()
        {
        }

        public static GetMaterialImportStatusQuery Create()
        {
            return new GetMaterialImportStatusQuery();
        }
    }

    public class GetMaterialImportStatusQueryResponse
    {
        public bool IsComplete { get; }
        public IList<ImportPage> Pages { get; }
        public DateTimeOffset CurrentCatalogPublished { get; }

        public GetMaterialImportStatusQueryResponse(bool isComplete, IList<ImportPage> pages, DateTimeOffset currentCatalogPublished)
        {
            IsComplete = isComplete;
            Pages = pages;
            CurrentCatalogPublished = currentCatalogPublished;
        }
    }

    public class ImportPage
    {
        public DateTimeOffset Published { get; }
        public int StartRow { get; }
        public int PageSize { get; }
        public bool IsLastPage { get; }
        public int ContentSize { get; }

        public ImportPage(DateTimeOffset published, int startRow, int pageSize, bool isLastPage, int contentSize)
        {
            Published = published;
            StartRow = startRow;
            PageSize = pageSize;
            IsLastPage = isLastPage;
            ContentSize = contentSize;
        }
    }

    internal class GetMaterialImportStatusQueryHandler : IQueryHandler<GetMaterialImportStatusQuery,
        GetMaterialImportStatusQueryResponse>
    {
        private readonly IMaterialImportPageRepository _importPageRepository;
        private readonly IMaterialRepository _materialRepository;

        public GetMaterialImportStatusQueryHandler(IMaterialImportPageRepository importPageRepository, IMaterialRepository materialRepository, IUnitOfWork unitOfWork)
        {
            _importPageRepository = importPageRepository;
            _materialRepository = materialRepository;
        }

        public async Task<GetMaterialImportStatusQueryResponse> Handle(GetMaterialImportStatusQuery notification,
            CancellationToken cancellationToken)
        {
            var latestCatalogPageInfo = await _importPageRepository.GetLatestCatalogPageInfo(cancellationToken);
            var isComplete = ImportChecker.IsImportComplete(latestCatalogPageInfo);

            var importPages = latestCatalogPageInfo.Select(p => new ImportPage(p.Published, p.StartRow, p.PageSize, p.IsLastPage, p.ContentSize))
                .ToList();
            
            var existingMaterialsPublished = await _materialRepository.GetPublished();
            
            return new GetMaterialImportStatusQueryResponse(isComplete, importPages, existingMaterialsPublished);
        }
    }
}
