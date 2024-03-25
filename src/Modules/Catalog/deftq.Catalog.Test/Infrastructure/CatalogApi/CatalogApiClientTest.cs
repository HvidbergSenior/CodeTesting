using deftq.Services.CatalogImport.Service.CatalogApi;
using deftq.Services.CatalogImport.Service.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace deftq.Catalog.Test.Infrastructure.CatalogApi
{
    public class CatalogApiClientTest
    {
        private readonly TestOutputLogger<CatalogApiClient> _testLogger;
        private string _apiKey = "<replace_me>";
        private Dependencies.CatalogApiConfig _catalogApiConfig;

        public CatalogApiClientTest(ITestOutputHelper output)
        {
            _testLogger = new TestOutputLogger<CatalogApiClient>(output);
            _catalogApiConfig = new Dependencies.CatalogApiConfig() {EVUTempoKey = _apiKey};
        }

        //[Fact]
        [Fact(Skip = "Manual test")]
        public async Task MaterialsDefaultPage()
        {
            using var httpClient = new HttpClient();
            var catalogApiFacade = new CatalogApiClient(httpClient, _catalogApiConfig, _testLogger);
            var materialCatalogResponse = await catalogApiFacade.FetchMaterialCatalogPageAsync(CancellationToken.None);

            Assert.NotNull(materialCatalogResponse.Data);
            Assert.Equal(1000, materialCatalogResponse.Data.Count);
            Assert.True(materialCatalogResponse.HasNextPage);
            Assert.False(materialCatalogResponse.HasPreviousPage);
        }

        //[Fact]
        [Fact(Skip = "Manual test")]
        public async Task MaterialsCustomPage()
        {
            using var httpClient = new HttpClient();
            var catalogApiFacade = new CatalogApiClient(httpClient, _catalogApiConfig, _testLogger);
            var materialCatalogResponse = await catalogApiFacade.FetchMaterialCatalogPageAsync(0, 10, CancellationToken.None);

            Assert.NotNull(materialCatalogResponse.Data);
            Assert.Equal(10, materialCatalogResponse.Data.Count);
            Assert.True(materialCatalogResponse.HasNextPage);
            Assert.False(materialCatalogResponse.HasPreviousPage);
        }

        //[Fact]
        [Fact(Skip = "Manual test")]
        public async Task AllMaterials()
        {
            using var httpClient = new HttpClient();
            var catalogApiFacade = new CatalogApiClient(httpClient, _catalogApiConfig, _testLogger);
            var materialCatalogResponse = await catalogApiFacade.FetchMaterialCatalogAsync(CancellationToken.None);

            Assert.NotNull(materialCatalogResponse);
            Assert.True(materialCatalogResponse.Count > 10000);
        }

        //[Fact]
        [Fact(Skip = "Manual test")]
        public async Task Operations()
        {
            using var httpClient = new HttpClient();
            var catalogApiFacade = new CatalogApiClient(httpClient, _catalogApiConfig, _testLogger);
            var operationCatalogResponse = await catalogApiFacade.FetchOperationCatalogAsync(CancellationToken.None);

            int operationIndex = 10;
            Assert.NotNull(operationCatalogResponse);
            Assert.True(operationCatalogResponse.Count > operationIndex);

            Assert.NotEmpty(operationCatalogResponse[operationIndex].OperationNumber);
            Assert.NotEmpty(operationCatalogResponse[operationIndex].Text);
            Assert.True(operationCatalogResponse[operationIndex].OperationTime > 0);
        }
        
        //[Fact]
        [Fact(Skip = "Manual test")]
        public async Task AllMaterialsCancellation()
        {
            using var tokenSource = new CancellationTokenSource();
            using var httpClient = new HttpClient();
            var catalogApiFacade = new CatalogApiClient(httpClient, _catalogApiConfig, _testLogger);
            var materialCatalogTask = catalogApiFacade.FetchMaterialCatalogAsync(tokenSource.Token);

            tokenSource.CancelAfter(TimeSpan.FromSeconds(1));
            var timeoutTask = Task.Delay(TimeSpan.FromSeconds(5));
            await Task.WhenAny(materialCatalogTask, timeoutTask);
            
            Assert.True(materialCatalogTask.Status != TaskStatus.Running);
        }
    }
}
