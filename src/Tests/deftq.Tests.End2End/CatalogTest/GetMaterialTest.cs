using System.Net.Http.Json;
using deftq.Catalog.Application.GetMaterial;
using Xunit;

namespace deftq.Tests.End2End.CatalogTest
{
    [Collection("End2End")]
    public class GetMaterialTest
    {
        private readonly WebAppFixture fixture;

        public GetMaterialTest(WebAppFixture webAppFixture)
        {
            fixture = webAppFixture;
        }

        [Fact]
        public async Task Get_Full_Material_Data()
        {
            await CatalogTestData.ImportMaterials(fixture);

            // Get full material info
            var materialId = CatalogTestData.MaterialWithReplacementId;
            var response = await fixture.Client.GetFromJsonAsync<GetMaterialResponse>($"/api/catalog/materials/{materialId}");
            Assert.NotNull(response);
            Assert.Equal(materialId, response?.Id);
            Assert.Equal("Gammelt 3x1,5 kabel", response?.Name);
            Assert.Equal("meter", response?.Unit);
            Assert.Equal("3333333333333", response?.EanNumber);
            Assert.Equal(1, response?.Mountings.Count);
            Assert.Equal(80, response?.Mountings[0].MountingCode);
            Assert.Equal("I/på beton", response?.Mountings[0].Text);
            Assert.Equal(125000, response?.Mountings[0].OperationTimeMilliseconds);
        }

        [Fact]
        public async Task Get_Full_Material_Data_With_Master_Material()
        {
            await CatalogTestData.ImportMaterials(fixture);

            // Get full material info
            var materialId = "D9EFABAB-A5F7-4E35-B640-6A91F2662667";
            var response = await fixture.Client.GetFromJsonAsync<GetMaterialResponse>($"/api/catalog/materials/{materialId}", fixture.JsonSerializerOptions());
            Assert.NotNull(response);
            Assert.Equal(Guid.Parse(materialId), response?.Id);
            Assert.Equal("Generelt 3x1,5 kabel med røde markeringer", response?.Name);
            Assert.Equal("meter", response?.Unit);
            Assert.Equal("2222222222222", response?.EanNumber);
            Assert.Equal(1, response?.Mountings.Count);
            Assert.Equal(25000, response?.Mountings[0].OperationTimeMilliseconds);
            
            // Get supplement operations from master material
            Assert.Equal(1, response?.Mountings[0].SupplementOperations.Count);
            Assert.Equal(Guid.Parse("80F99085-A142-4ED5-91B3-A5D310B86DA3"), response?.Mountings[0].SupplementOperations[0].SupplementOperationId);
            Assert.Equal("Master operation", response?.Mountings[0].SupplementOperations[0].Text);
        }
        
        [Fact]
        public async Task Get_Full_Material_Data_With_Supplement_Operations()
        {
            await CatalogTestData.ImportMaterials(fixture);

            // Get full material info
            var materialId = "4EAE982E-21BE-4596-8039-DA8A45BB7FD7";
            var response = await fixture.Client.GetFromJsonAsync<GetMaterialResponse>($"/api/catalog/materials/{materialId}", fixture.JsonSerializerOptions());
            Assert.NotNull(response);
            Assert.Equal(Guid.Parse(materialId), response?.Id);
            Assert.Equal(1, response?.Mountings.Count);
            Assert.True(response?.Mountings[0].SupplementOperations.Count > 2);
            
            var embeddedOperation = response?.Mountings[0].SupplementOperations
                .FirstOrDefault(o => o.SupplementOperationId == Guid.Parse("DF554BDD-90E0-4883-A485-C117CA27BC87"));
            Assert.Equal("Just do something", embeddedOperation?.Text);
            Assert.Equal(29000m, embeddedOperation?.OperationTimeMilliseconds);
            
            var referencedOperation = response?.Mountings[0].SupplementOperations
                .FirstOrDefault(o => o.SupplementOperationId == Guid.Parse("3F65DA66-C36E-4BB7-9CB8-D968D4783AFF"));
            Assert.Equal("Dig a hole", referencedOperation?.Text);
            Assert.Equal(31000m, referencedOperation?.OperationTimeMilliseconds);
        }
    }
}
