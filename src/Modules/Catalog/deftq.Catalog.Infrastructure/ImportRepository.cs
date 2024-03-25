using System.Transactions;
using deftq.Catalog.Domain.MaterialImport;
using Marten;
using Marten.Services;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace deftq.Catalog.Infrastructure
{
    public class ImportRepository : IImportRepository
    {
        private readonly IDocumentStore _documentStore;
        private readonly ILogger<ImportRepository> _logger;

        public ImportRepository(IDocumentStore documentStore, ILogger<ImportRepository> logger)
        {
            _documentStore = documentStore;
            _logger = logger;
        }

        public async Task SwitchMaterialCatalog(DateTimeOffset catalogPublished, CancellationToken cancellationToken)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await using var session = _documentStore.OpenSession(SessionOptions.ForCurrentTransaction());

            // Delete all existing materials
            await using var deleteCommand = new NpgsqlCommand("DELETE FROM public.mt_doc_material");
            await session.ExecuteAsync(deleteCommand, cancellationToken);

            // Import new catalog from pages
            var insertCommand =
                @"SELECT public.mt_insert_material(uuid('11111111-1111-1111-1111-111111111111'), elem, 'deftq.Catalog.Domain.MaterialCatalog.Material', (elem->>'Id')::uuid, uuid('11111111-1111-1111-1111-111111111111')) 
                    FROM (
                      SELECT jsonb_array_elements (data->'Content'->'$values') as elem 
                      FROM mt_doc_materialimportpage 
                      WHERE CAST(data->>'Published' AS timestamptz) = $1
                    ) elements";

            await using var npgsqlCommand = new NpgsqlCommand(insertCommand)
            {
                Parameters = { new() { Value = catalogPublished } }
            };
            await session.ExecuteAsync(npgsqlCommand, cancellationToken);

            scope.Complete();
        }
    }
}
