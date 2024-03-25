namespace deftq.Services.CatalogImport.Service.CatalogApi
{
    public interface ICatalogApiClient
    {
        Task<DateTimeOffset> CatalogPublishedAsync(CancellationToken cancellationToken);
        Task<IList<MaterialResponse>> FetchMaterialCatalogAsync(CancellationToken cancellationToken);
        Task<IList<OperationResponse>> FetchOperationCatalogAsync(CancellationToken cancellationToken);
        Task<MaterialCatalogResponse> FetchMaterialCatalogPageAsync(int startRow, int pageSize, CancellationToken cancellationToken);
    }
}
