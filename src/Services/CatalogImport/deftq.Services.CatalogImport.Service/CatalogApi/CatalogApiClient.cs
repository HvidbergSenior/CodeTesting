using System.Globalization;
using System.Net;
using System.Text.Json;
using deftq.Catalog.Domain.CatalogApi;
using Microsoft.Extensions.Logging;

namespace deftq.Services.CatalogImport.Service.CatalogApi
{
    public class CatalogApiClient : ICatalogApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CatalogApiClient> _logger;
        private readonly string _apiKey;
        private readonly string _apiKeyHeader;
        private readonly string _apiMaterialEndpointUrl;
        private readonly string _apiMaterialEndpointParameters = "?start_row={0}&page_size={1}";
        private readonly string _apiOperationEndpointUrl;
        private static readonly DateTimeOffset CatalogPublishedTimestamp = new DateTimeOffset(2023, 1, 1, 1, 0, 0, TimeSpan.Zero);
        
        public CatalogApiClient(HttpClient httpClient, ICatalogApiConfig catalogApiConfig, ILogger<CatalogApiClient> logger)
        {
            _httpClient = httpClient;
            _apiKey = catalogApiConfig.EVUTempoKey;
            _apiKeyHeader = catalogApiConfig.EVUTempoKeyHeader;
            _apiMaterialEndpointUrl = catalogApiConfig.EVUTempoMaterialEndpointUrl;
            _apiOperationEndpointUrl = catalogApiConfig.EVUTempoOperationEndpointUrl;
            _logger = logger;
        }

        public Task<DateTimeOffset> CatalogPublishedAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(CatalogPublishedTimestamp);
        }

        public async Task<IList<MaterialResponse>> FetchMaterialCatalogAsync(CancellationToken cancellationToken)
        {
            var result = new List<MaterialResponse>();
            var page = await FetchMaterialCatalogPageAsync(cancellationToken);
            result.AddRange(page.Data);
            while (!cancellationToken.IsCancellationRequested && page.HasNextPage)
            {
                page = await FetchMaterialCatalogPageAsync(page.NextPageUrl, cancellationToken);
                result.AddRange(page.Data);
                var materialWithMountings = page.Data.Where(d => d.Mountings.Count > 0).ToList();
                if (materialWithMountings.Count > 0)
                {
                    var count = materialWithMountings.Count;
                    _logger.LogInformation("{Count} materials with mountings", count);

                    var materialWithMounting = materialWithMountings.First();
                    var ean = materialWithMounting.Ean;
                    var mountingsCount = materialWithMounting.Mountings.Count;
                    _logger.LogInformation("{Ean} mountings {MountingsCount}", ean, mountingsCount);
                }
            }

            return result;
        }

        public async Task<MaterialCatalogResponse> FetchMaterialCatalogPageAsync(CancellationToken cancellationToken)
        {
            using var httpRequestMessage = new HttpRequestMessage();
            var requestUri = new Uri(_apiMaterialEndpointUrl);
            return await FetchMaterialCatalogPageAsync(requestUri, cancellationToken);
        }

        public async Task<MaterialCatalogResponse> FetchMaterialCatalogPageAsync(int startRow, int pageSize, CancellationToken cancellationToken)
        {
            var uriString = String.Format(CultureInfo.InvariantCulture, _apiMaterialEndpointUrl + _apiMaterialEndpointParameters, startRow, pageSize);
            var requestUri = new Uri(uriString);
            return await FetchMaterialCatalogPageAsync(requestUri, cancellationToken);
        }

        private async Task<MaterialCatalogResponse> FetchMaterialCatalogPageAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching page {RequestUri}", requestUri);
            using var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.Add(_apiKeyHeader, _apiKey);
            httpRequestMessage.RequestUri = requestUri;
            using var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            return await HandleResponse<MaterialCatalogResponse>(response);
        }

        public async Task<IList<OperationResponse>> FetchOperationCatalogAsync(CancellationToken cancellationToken)
        {
            using var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Headers.Add(_apiKeyHeader, _apiKey);
            var uriString = _apiOperationEndpointUrl;
            var requestUri = new Uri(uriString);
            httpRequestMessage.RequestUri = requestUri;
            using var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            return await HandleResponse<List<OperationResponse>>(response);
        }

        private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new CatalogApiException($"Unexpected status code {response.StatusCode}");
            }

            var contentString = await response.Content.ReadAsStringAsync();
            var responseDeserialized = JsonSerializer.Deserialize<T>(contentString);

            if (responseDeserialized is null)
            {
                throw new CatalogApiException("Unable to deserialize response content");
            }

            return responseDeserialized;
        }
    }

    public interface ICatalogApiConfig
    {
        string EVUTempoKey { get; }
        
        string EVUTempoKeyHeader { get; }
        
        string EVUTempoMaterialEndpointUrl { get; }
        
        string EVUTempoOperationEndpointUrl { get; }
    }
}
