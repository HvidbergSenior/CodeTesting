using deftq.BuildingBlocks.Application.Commands;
using deftq.BuildingBlocks.Application.Queries;
using deftq.Catalog.Application.Import.GetMaterialImportStatus;
using deftq.Catalog.Application.Import.ImportMaterialCatalog;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Service
{
    public class CatalogImportService : ICatalogImportService
    {
        private readonly ILogger<CatalogImportService> _logger;
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;

        public CatalogImportService(ILogger<CatalogImportService> logger, IQueryBus queryBus, ICommandBus commandBus)
        {
            _logger = logger;
            _queryBus = queryBus;
            _commandBus = commandBus;
        }

        public async Task ImportCatalog(CancellationToken cancellationToken)
        {
            var query = GetMaterialImportStatusQuery.Create();
            var importStatus = await _queryBus.Send<GetMaterialImportStatusQuery, GetMaterialImportStatusQueryResponse>(query);

            if (!importStatus.IsComplete)
            {
                _logger.LogDebug("Latest catalog import is not complete... skipping");
                return;
            }
            
            var catalogPublished = GetPublished(importStatus.Pages);
            var existingMaterialsPublished = importStatus.CurrentCatalogPublished;

            if (catalogPublished == existingMaterialsPublished)
            {
                _logger.LogDebug("Already imported catalog published {Published}", catalogPublished);
                return;
            }
            
            var cmd = ImportMaterialCatalogCommand.Create(catalogPublished);
            await _commandBus.Send(cmd, cancellationToken);
        }
        
        private static DateTimeOffset GetPublished(IList<ImportPage> latestCatalogPageInfo)
        {
            var firstPage = latestCatalogPageInfo.FirstOrDefault();
            if (firstPage is null)
            {
                throw new InvalidOperationException("Import marked as complete, but contained no pages");
            }

            return firstPage.Published;
        }
    }
}
