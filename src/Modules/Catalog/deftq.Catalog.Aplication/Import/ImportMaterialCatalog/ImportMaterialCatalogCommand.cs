using System.Transactions;
using deftq.BuildingBlocks.Application.Commands;
using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Catalog.Domain.MaterialImport;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace deftq.Catalog.Application.Import.ImportMaterialCatalog
{
    public sealed class ImportMaterialCatalogCommand : ICommand<ICommandResponse>
    {
        public DateTimeOffset Published { get; }

        private ImportMaterialCatalogCommand(DateTimeOffset published)
        {
            Published = published;
        }

        public static ImportMaterialCatalogCommand Create(DateTimeOffset published)
        {
            return new ImportMaterialCatalogCommand(published);
        }
    }

    internal class ImportMaterialCatalogCommandHandler : ICommandHandler<ImportMaterialCatalogCommand, ICommandResponse>
    {
        private readonly IImportRepository _importRepository;
        private readonly ILogger<ImportMaterialCatalogCommand> _logger;

        public ImportMaterialCatalogCommandHandler(IImportRepository importRepository, ILogger<ImportMaterialCatalogCommand> logger)
        {
            _importRepository = importRepository;
            _logger = logger;
        }

        public async Task<ICommandResponse> Handle(ImportMaterialCatalogCommand request, CancellationToken cancellationToken)
        {
            var catalogPublished = request.Published;
            
            _logger.LogInformation("Importing all pages from catalog published {Published}", catalogPublished);

            await _importRepository.SwitchMaterialCatalog(catalogPublished, cancellationToken);
            
            return new EmptyCommandResponse();
        }
    }
}
