using System.Text;
using deftq.BuildingBlocks.DataAccess.Marten;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.ProjectCatalogFavorite
{
    public class ProjectCatalogFavoriteListTest
    {
        [Fact]
        public void CanSerialize()
        {
            // Given
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), Any.ProjectId());
            favorites.AddFavorite(Any.CatalogFavorite());

            // When
            var serializer = Registration.GetJsonNetSerializer();
            var json = serializer.ToJson(favorites);
            using var sr = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var favoritesDeserialized = serializer.FromJson<ProjectCatalogFavoriteList>(sr);

            // Then
            favoritesDeserialized.Favorites.Should().ContainSingle();
        }

        [Fact]
        public void GivenFavoriteList_WhenAddingFavorite_ListShouldIncludeFavorite()
        {
            // Given
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), Any.ProjectId());

            // When
            var catalogFavorite = Any.CatalogFavorite();
            favorites.AddFavorite(catalogFavorite);

            // Then
            var favorite = favorites.Favorites.Should().ContainSingle().Subject;
            favorite.Should().BeEquivalentTo(catalogFavorite);
        }

        [Fact]
        public void GivenFavoriteList_WhenAddingSameFavoriteAgain_ExceptionIsThrown()
        {
            // Given
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), Any.ProjectId());

            // When
            var catalogFavorite = Any.CatalogFavorite();
            favorites.AddFavorite(catalogFavorite);

            // Then
            Assert.Throws<ProjectCatalogFavoriteAlreadyAddedException>(() => favorites.AddFavorite(catalogFavorite));
        }

        [Fact]
        public void GivenFavoriteList_WhenRemovingFavorite_FavoriteIsNotInList()
        {
            // Given
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), Any.ProjectId());
            var catalogFavorite = Any.CatalogFavorite();
            favorites.AddFavorite(catalogFavorite);
            
            // When
            favorites.RemoveFavorite(catalogFavorite.CatalogFavoriteId);

            // Then
            favorites.Favorites.Should().BeEmpty();
        }
        
        [Fact]
        public void GivenFavoriteList_WhenRemovingUnknownFavorite_ExceptionIsThrown()
        {
            // Given
            var favorites = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), Any.ProjectId());
            
            // Then
            Assert.Throws<ProjectCatalogFavoriteNotFoundException>(() => favorites.RemoveFavorite(Any.CatalogFavoriteId()));
        }
    }
}
