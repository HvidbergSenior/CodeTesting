using deftq.Catalog.Domain.MaterialCatalog;
using deftq.Services.CatalogImport.Service;
using Xunit;

namespace deftq.Services.CatalogImport.Test
{
    [Collection("Services.End2End")]
    public class End2EndTest
    {
        private readonly ServiceFixture _fixture;
        public const int MaterialCount = 250;

        public End2EndTest(ServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenFakeApi_WhenRunningServices_MaterialsAreImported()
        {
            var materialRepository = (IMaterialRepository)_fixture.Host.Services.GetService(typeof(IMaterialRepository))!;
            var fetchService = (ICatalogFetchService)_fixture.Host.Services.GetService(typeof(ICatalogFetchService))!;
            var importService = (ICatalogImportService)_fixture.Host.Services.GetService(typeof(ICatalogImportService))!;
            
            Assert.Empty(materialRepository.Query().ToList());
            
            await fetchService.FetchCatalog(CancellationToken.None);
            await fetchService.FetchCatalog(CancellationToken.None);
            await fetchService.FetchCatalog(CancellationToken.None);

            await importService.ImportCatalog(CancellationToken.None);
            
            Assert.Equal(MaterialCount, materialRepository.Query().ToList().Count);
        }
    }
}
