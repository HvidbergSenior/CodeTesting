using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.projectCatalogFavorite;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Domain.projectCatalogFavorite
{
    public class ProjectRemovedEventHandlerTest
    {
        [Fact]
        public async Task ShouldNotReturnFavoritesWhenProjectIsDeleted()
        {
            var uow = new FakeUnitOfWork();
            var favoriteListRepository = new ProjectCatalogFavoriteListInMemoryRepository();

            var favorites = Any.ProjectCatalogFavoriteList();

            await favoriteListRepository.Add(favorites);
            
            var handler = new ProjectRemovedEventHandler(favoriteListRepository, uow);
            var evt = ProjectRemovedDomainEvent.Create(favorites.ProjectId);
            
            await handler.Handle(evt, CancellationToken.None);
            
            Assert.Empty(favoriteListRepository.Entities);
            Assert.True(favoriteListRepository.SaveChangesCalled);
        }
    }
}
