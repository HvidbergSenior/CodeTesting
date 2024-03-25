using deftq.Services.CatalogImport.Service.CatalogApi;

namespace deftq.Services.CatalogImport.Test
{
    public class FakeCatalogApiClient : ICatalogApiClient
    {
        private readonly IList<MaterialResponse> _materialResponses;
        private readonly DateTimeOffset _published = new(2023, 2,1, 0, 0, 0, TimeSpan.Zero);
        private Uri _emptyUrl = new("http://fake.com");

        public FakeCatalogApiClient(IList<MaterialResponse> materialResponses)
        {
            _materialResponses = materialResponses;
        }

        public Task<DateTimeOffset> CatalogPublishedAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_published);
        }

        public Task<IList<MaterialResponse>> FetchMaterialCatalogAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_materialResponses);
        }

        public Task<IList<OperationResponse>> FetchOperationCatalogAsync(CancellationToken cancellationToken)
        {
            IList<OperationResponse> operationResponses = new List<OperationResponse>();
            return Task.FromResult(operationResponses);
        }

        public Task<MaterialCatalogResponse> FetchMaterialCatalogPageAsync(int startRow, int pageSize, CancellationToken cancellationToken)
        {
            var materials = _materialResponses.Skip(startRow-1).Take(pageSize).ToList();
            var hasNextPage = _materialResponses.Skip((startRow - 1) + pageSize).Any();
            var response = new MaterialCatalogResponse(hasNextPage, startRow > 1, _emptyUrl, _emptyUrl, materials);
            return Task.FromResult(response);
        }
    }
}
