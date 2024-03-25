using System.Net;
using System.Text.Json;
using deftq.Akkordplus.WebApplication.Controllers.Catalog.SearchMaterials;
using deftq.Catalog.Application.SearchMaterial;
using Xunit;

namespace deftq.Tests.End2End.CatalogTest
{
    [Collection("End2End")]
    public class SearchMaterialsTest
    {
        private readonly WebAppFixture fixture;
        
        public SearchMaterialsTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
        }
        
        [Fact]
        public async Task SimpleNameSearch()
        {
            await CatalogTestData.ImportMaterials(fixture);
            
            var searchResponse = await Search("SimpElt", 10);
            Assert.Single(searchResponse.FoundMaterials);
            
            searchResponse = await Search("simpelt", 10);
            Assert.Single(searchResponse.FoundMaterials);
        }
        
        [Fact]
        public async Task BasicFormNameSearch()
        {
            await CatalogTestData.ImportMaterials(fixture);
            
            var searchResponse = await Search("rød", 10);
            Assert.Single(searchResponse.FoundMaterials.Where(m => m.Name.Equals("Generelt 3x1,5 kabel med røde markeringer", StringComparison.Ordinal)));
        }
        
        [Fact]
        public async Task PartialEanNumberSearch()
        { 
            await CatalogTestData.ImportMaterials(fixture);
            
            var searchResponse = await Search("112211", 10);
            Assert.Single(searchResponse.FoundMaterials);
        }
        
        [Fact]
        public async Task AllSearch()
        {
            await CatalogTestData.ImportMaterials(fixture);
         
            var searchResponse = await Search("", 10);
            Assert.True(searchResponse.FoundMaterials.Count >= 3);
        }
        
        private async Task<SearchMaterialResponse> Search(string query, uint maxHits)
        {
            // Search materials
            var request = new SearchMaterialsRequest(query, maxHits);
            var rawResponse = await fixture.Client.PostAsJsonAsync("/api/catalog/materials/search", request);
            
            Assert.Equal(HttpStatusCode.OK, rawResponse.StatusCode);
            await using var stream = await rawResponse.Content.ReadAsStreamAsync();
            return JsonSerializer.Deserialize<SearchMaterialResponse>(stream, fixture.JsonSerializerOptions())!;
        }
    }
}
