using deftq.Pieceworks.Application.GetFavoriteList;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectFavoriteList
{
    public class GetProjectFavoriteListQueryTest
    {
        private readonly ProjectCatalogFavoriteListInMemoryRepository _projectCatalogFavoriteListRepository;

        public GetProjectFavoriteListQueryTest()
        {
            _projectCatalogFavoriteListRepository = new ProjectCatalogFavoriteListInMemoryRepository();
        }
        
        [Fact]
        public async Task Should_Get_Favorite_List_From_Project()
        {
            var project = Any.Project();
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), project.ProjectId);
            var catalogFavorite = Any.CatalogFavorite();
            favorites.AddFavorite(catalogFavorite);
            await _projectCatalogFavoriteListRepository.Add(favorites);

            var handler = new GetProjectFavoriteListQueryHandler(_projectCatalogFavoriteListRepository);
            var favoriteQuery = GetProjectFavoriteListQuery.Create(project.ProjectId.Value);
            var favoriteResponse = await handler.Handle(favoriteQuery, CancellationToken.None);
            
            Assert.Equal(catalogFavorite.CatalogItemId.Value, favoriteResponse.Favorites[0].CatalogId);
            Assert.Equal(catalogFavorite.CatalogItemNumber.Value, favoriteResponse.Favorites[0].Number);
            Assert.Equal(catalogFavorite.CatalogItemText.Value, favoriteResponse.Favorites[0].Text);
        }
    }
}
