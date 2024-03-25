using System.Net.Http.Json;
using deftq.Catalog.Application.GetSupplements;
using Xunit;

namespace deftq.Tests.End2End.CatalogTest
{
    [Collection("End2End")]
    public class GetSupplementsTest
    {
        private readonly WebAppFixture fixture;
        
        public GetSupplementsTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
        }
        
        [Fact]
        public async Task Get_Supplements()
        {
            await CatalogTestData.ImportSupplements(fixture);
        
            // Get list of available supplements
            var response = await fixture.Client.GetFromJsonAsync<GetSupplementsResponse>("/api/catalog/supplements");
            
            // Assert that the 10 default supplements are returned
            Assert.NotNull(response);
            Assert.True(response?.Supplements.Count >= 2);
        }
    }
}
