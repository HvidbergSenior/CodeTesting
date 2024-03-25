using System.Net;
using System.Text.Json;
using Baseline;
using deftq.Akkordplus.WebApplication.Controllers.Catalog.SearchOperations;
using deftq.Catalog.Application.SearchOperation;
using Xunit;

namespace deftq.Tests.End2End.CatalogTest
{
    [Collection("End2End")]
    public class SearchOperationsTest
    {
        private readonly WebAppFixture fixture;
        
        public SearchOperationsTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
        }

        [Fact]
        public async Task SimpleTextSearch()
        {
            await CatalogTestData.ImportOperations(fixture);

            var searchResponse = await Search("dæksel", 100);
            Assert.True(searchResponse.FoundOperations.Count >= 1);
            Assert.Contains(searchResponse.FoundOperations, o => o.OperationNumber.EqualsIgnoreCase("170141000001"));
            Assert.Contains(searchResponse.FoundOperations, o => o.OperationText.EqualsIgnoreCase("Fjerne dæksel"));
        }
        
        [Fact]
        public async Task SimpleNumberSearch()
        {
            await CatalogTestData.ImportOperations(fixture);

            var searchResponse = await Search("17020000", 100);
            Assert.True(searchResponse.FoundOperations.Count == 1);
            Assert.Contains(searchResponse.FoundOperations, o => o.OperationNumber.EqualsIgnoreCase("170200000001"));
            Assert.Contains(searchResponse.FoundOperations, o => o.OperationText.EqualsIgnoreCase("Hul for 2M Euro-dåse i gips <=15mm/1 lag, Ø o/60-80 mm"));
        }
        
        private async Task<SearchOperationResponse> Search(string query, uint maxHits)
        {
            // Search operations
            var request = new SearchOperationsRequest(query, maxHits);
            var rawResponse = await fixture.Client.PostAsJsonAsync("/api/catalog/operations/search", request);
            
            Assert.Equal(HttpStatusCode.OK, rawResponse.StatusCode);
            await using var stream = await rawResponse.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<SearchOperationResponse>(stream, fixture.JsonSerializerOptions())!;
        }
    }
}
