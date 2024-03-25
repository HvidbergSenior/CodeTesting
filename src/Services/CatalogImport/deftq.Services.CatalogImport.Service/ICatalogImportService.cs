namespace deftq.Services.CatalogImport.Service
{
    public interface ICatalogImportService
    {
        Task ImportCatalog(CancellationToken cancellationToken);
    }
}
