using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.Import.CreateMaterialImportPage;
using deftq.Catalog.Application.Import.GetMaterialImportStatus;
using deftq.Services.CatalogImport.Service.CatalogApi;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Service
{
    public class CatalogFetchService : ICatalogFetchService
    {
        private const int DefaultPageSize = 100;
        private const int DefaultStartRow = 1;

        private readonly ICatalogApiClient _apiClient;
        private readonly ILogger<CatalogFetchService> _logger;
        
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;

        public CatalogFetchService(ICatalogApiClient apiClient, ILogger<CatalogFetchService> logger, IQueryBus queryBus, ICommandBus commandBus)
        {
            _apiClient = apiClient;
            _logger = logger;
            _queryBus = queryBus;
            _commandBus = commandBus;
        }

        public async Task<MaterialCatalogResponse> FetchCatalog(CancellationToken cancellationToken)
        {
            var currentCatalogPublished = await _apiClient.CatalogPublishedAsync(cancellationToken);
            var query = GetMaterialImportStatusQuery.Create();
            var importStatus = await _queryBus.Send<GetMaterialImportStatusQuery, GetMaterialImportStatusQueryResponse>(query);

            var pageToFetch = GetPageToFetch(importStatus);
            
            if (pageToFetch.Published is null || currentCatalogPublished > pageToFetch.Published.Value)
            {
                _logger.LogInformation("Catalog published {CurrentCatalogPublished}, fetching start row 0, page size {DefaultPageSize}",
                    currentCatalogPublished, DefaultPageSize);

                var page = await _apiClient.FetchMaterialCatalogPageAsync(DefaultStartRow, DefaultPageSize, cancellationToken);
                var mappedMaterials = MapMaterials(page.Data);
                var cmd = CreateMaterialImportPageCommand.Create(currentCatalogPublished, DefaultStartRow, DefaultPageSize, !page.HasNextPage, mappedMaterials);
                await _commandBus.Send(cmd, cancellationToken);
                return page;
            }
            
            if (pageToFetch.PageSize > 0)
            {
                var startRow = pageToFetch.StartRow;
                var pageSize = pageToFetch.PageSize;

                _logger.LogInformation("Catalog published {CurrentCatalogPublished}, fetching start row {StartRow}, page size {PageSize}",
                    currentCatalogPublished, startRow, pageSize);

                var page = await _apiClient.FetchMaterialCatalogPageAsync(startRow, pageSize, cancellationToken);
                var mappedMaterials = MapMaterials(page.Data);
                var cmd = CreateMaterialImportPageCommand.Create(currentCatalogPublished, startRow, pageSize, !page.HasNextPage, mappedMaterials);
                await _commandBus.Send(cmd, cancellationToken);
                return page;
            }

            return MaterialCatalogResponse.Empty();
        }

        private class PageToFetch
        {
            public DateTimeOffset? Published { get; }
            public int StartRow { get; }
            public int PageSize { get; }

            public PageToFetch(DateTimeOffset? published, int startRow, int pageSize)
            {
                Published = published;
                StartRow = startRow;
                PageSize = pageSize;
            }
        }
        
        private static PageToFetch GetPageToFetch(GetMaterialImportStatusQueryResponse importStatus)
        {
            var latestPage = importStatus.Pages.MaxBy(p => p.StartRow);

            if (latestPage is not null)
            {
                if (latestPage.IsLastPage)
                {
                    // Last page, must mean we have all materials
                    return new PageToFetch(latestPage.Published, -1, -1);
                }

                // Not last page, there must be more pages to fetch
                var nextPageStartRow = latestPage.StartRow + latestPage.PageSize;
                return new PageToFetch(latestPage.Published, nextPageStartRow, latestPage.PageSize);
            }

            // No pages stored
            return new PageToFetch(null, -1, -1);
        }

        private static IList<ImportMaterial> MapMaterials(IList<MaterialResponse> pageData)
        {
            return pageData.Select(m => new ImportMaterial(m.Ean, m.Name, m.Unit, MapMountings(m.Mountings))).ToList();
        }

        private static IList<ImportMounting> MapMountings(IList<MountingResponse> mountings)
        {
            return mountings.Select(m => new ImportMounting(m.MountingCodeAsInt(), m.OperationTimeMs, MapSupplementOperations(m.SupplementOperations)))
                .ToList();
        }

        private static IList<ImportSupplementOperation> MapSupplementOperations(IList<SupplementOperation> supplementOperations)
        {
            return supplementOperations.Select(op => new ImportSupplementOperation(op.OperationNumber, op.Text, op.OperationTimeMs, op.Type))
                .ToList();
        }
    }
}
