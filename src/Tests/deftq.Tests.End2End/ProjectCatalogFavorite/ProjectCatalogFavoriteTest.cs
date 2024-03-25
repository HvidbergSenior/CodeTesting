using deftq.Akkordplus.WebApplication.Controllers.Pieceworks.RegisterProjectCatalogFavorite;
using deftq.Pieceworks.Application.GetFavoriteList;
using deftq.Tests.End2End.CatalogTest;
using Xunit;

namespace deftq.Tests.End2End.ProjectCatalogFavorite
{
    [Collection("End2End")]
    public class ProjectCatalogFavoriteTest
    {
        private readonly WebAppFixture _fixture;
        private readonly Api _api;

        public ProjectCatalogFavoriteTest(WebAppFixture webAppFixture)
        {
            _fixture = webAppFixture;
            _api = new Api(webAppFixture);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringFavoriteMaterial_FavoriteIsAddedToList()
        {
            //Import materials from catalog
            await CatalogTestData.ImportMaterials(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Register favorite
            await _api.RegisterFavorite(projectId, CatalogTestData.MaterialWithMasterId, FavoriteCatalogType.Material);

            //Get favorite list
            var favoriteResponse = await _api.GetFavorites(projectId);
            Assert.Equal("meter", favoriteResponse.Favorites[0].Unit);
            Assert.Equal(CatalogItemType.Material, favoriteResponse.Favorites[0].CatalogType);
            Assert.Equal(CatalogTestData.MaterialWithMasterId, favoriteResponse.Favorites[0].CatalogId);
            Assert.Equal(CatalogTestData.MaterialWithMasterName, favoriteResponse.Favorites[0].Text);
        }

        [Fact]
        public async Task GivenProject_WhenRegisteringFavoriteOperation_FavoriteIsAddedToList()
        {
            //Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();
            
            // Register favorite
            await _api.RegisterFavorite(projectId, CatalogTestData.BoxHoleOperationId, FavoriteCatalogType.Operation);
            
            //Get favorite list
            var favoriteResponse = await _api.GetFavorites(projectId);
            Assert.Equal("", favoriteResponse.Favorites[0].Unit);
            Assert.Equal(CatalogItemType.Operation, favoriteResponse.Favorites[0].CatalogType);
            Assert.Equal(CatalogTestData.BoxHoleOperationId, favoriteResponse.Favorites[0].CatalogId);
            Assert.Equal(CatalogTestData.BoxHoleOperationName, favoriteResponse.Favorites[0].Text);
        }

        [Fact]
        public async Task WhenRemovingFavorite_FavoriteShouldBeRemoved()
        {
            //Import materials from catalog
            await CatalogTestData.ImportOperations(_fixture);

            // Create a project
            var projectId = await _api.CreateProject();

            // Register favorite
            await _api.RegisterFavorite(projectId, CatalogTestData.BoxHoleOperationId, FavoriteCatalogType.Operation);
            
            //Get favorite list
            var favoriteResponse = await _api.GetFavorites(projectId);

            //Delete favorite from list
            await _api.RemoveFavorite(projectId, favoriteResponse.Favorites[0].FavoriteItemId);
            
            //Assert favorite is deleted from list
            favoriteResponse = await _api.GetFavorites(projectId);
            Assert.Empty(favoriteResponse.Favorites);
        }
    }
}
