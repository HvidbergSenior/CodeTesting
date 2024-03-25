using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectCatalogFavorite;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectCatalogFavorite
{
    public class RegisterProjectCatalogFavoriteCommandHandlerTest
    {
        [Fact]
        public async Task GivenProject_WhenRegisteringCatalogFavorite_FavoriteIsInList()
        {
            // Given
            var fakeUnitOfWork = new FakeUnitOfWork();
            var repo = new ProjectCatalogFavoriteListInMemoryRepository();
            var projectId = Any.ProjectId();
            await repo.Add(ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), projectId));

            // When
            var handler = new RegisterProjectCatalogFavoriteCommandHandler(repo, fakeUnitOfWork);
            var catalogFavoriteId = Any.Guid();
            var catalogItemType = CatalogItemType.Material;
            var catalogItemId = Any.Guid();
            var catalogItemNumber = "1234567890123";
            var catalogItemText = "Some material from the catalog";
            var catalogItemUnit = "123";
            var cmd = RegisterProjectCatalogFavoriteCommand.Create(projectId.Value, catalogFavoriteId, catalogItemType, catalogItemId, catalogItemNumber, catalogItemText, catalogItemUnit);
            await handler.Handle(cmd, CancellationToken.None);
            
            // Then favorite list should contain single item
            repo.Entities.Should().ContainSingle();
            repo.SaveChangesCalled.Should().BeTrue();
            fakeUnitOfWork.IsCommitted.Should().BeTrue();
            var favoriteList = await repo.GetByProjectId(projectId.Value);
            favoriteList.Favorites.Should().ContainSingle();
            
            // And favorite item should be as expected
            var favorite = favoriteList.Favorites.Single();
            favorite.CatalogFavoriteId.Value.Should().Be(catalogFavoriteId);
            favorite.CatalogItemId.Value.Should().Be(catalogItemId);
            favorite.CatalogItemNumber.Value.Should().Be(catalogItemNumber);
            favorite.CatalogItemText.Value.Should().Be(catalogItemText);
            favorite.CatalogItemType.Should().Be(catalogItemType);
        }
    }
}
