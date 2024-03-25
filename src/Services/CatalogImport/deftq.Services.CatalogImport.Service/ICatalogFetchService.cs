using deftq.Services.CatalogImport.Service.CatalogApi;

namespace deftq.Services.CatalogImport.Service
{
    public interface ICatalogFetchService
    {
        Task<MaterialCatalogResponse> FetchCatalog(CancellationToken cancellationToken);
    }
}
