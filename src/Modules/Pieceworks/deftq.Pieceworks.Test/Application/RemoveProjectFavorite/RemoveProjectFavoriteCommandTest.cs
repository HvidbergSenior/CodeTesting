using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveFromProjectFavorite;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectFavorite
{
    public class RemoveProjectFavoriteCommandTest
    {
        private readonly FakeUnitOfWork _unitOfWork;
        private readonly FakeExecutionContext _executionContext;
        private readonly ProjectCatalogFavoriteListInMemoryRepository _projectCatalogFavoriteListRepository;

        public RemoveProjectFavoriteCommandTest()
        {
            _unitOfWork = new FakeUnitOfWork();
            _executionContext = new FakeExecutionContext();
            _projectCatalogFavoriteListRepository = new ProjectCatalogFavoriteListInMemoryRepository();
        }

        [Fact]
        public async Task RemoveMultipleProjectFavorites_ShouldNotReturnProjectFavorites()
        {
            var project = Any.Project();
            var favoriteList = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), project.ProjectId);
            var catalogFavorite = Any.CatalogFavorite();
            var catalogFavorite2 = Any.CatalogFavorite();
            favoriteList.AddFavorite(catalogFavorite);
            favoriteList.AddFavorite(catalogFavorite2);
            await _projectCatalogFavoriteListRepository.Add(favoriteList);

            var handler = new RemoveFromProjectFavoriteListCommandHandler(_projectCatalogFavoriteListRepository, _unitOfWork, _executionContext);
            var cmd = RemoveProjectFavoriteCommand.Create(project.ProjectId.Value, 
                new List<Guid> { catalogFavorite.CatalogFavoriteId.Value, catalogFavorite2.CatalogFavoriteId.Value });
            await handler.Handle(cmd, CancellationToken.None);

            Assert.Single(_projectCatalogFavoriteListRepository.Entities);
            Assert.True(_projectCatalogFavoriteListRepository.SaveChangesCalled);
            Assert.True(_unitOfWork.IsCommitted);

            favoriteList = await _projectCatalogFavoriteListRepository.GetByProjectId(project.ProjectId.Value);
            Assert.Empty(favoriteList.Favorites);
        }

        [Fact]
        public async Task RemoveUnknownProjectFavorite_ShouldThrow()
        {
            var project = Any.Project();
            var favoriteList = ProjectCatalogFavoriteList.Create(Any.ProjectCatalogFavoriteListId(), project.ProjectId);
            var catalogFavorite = Any.CatalogFavorite();
            var catalogFavorite2 = Any.CatalogFavorite();
            favoriteList.AddFavorite(catalogFavorite);
            favoriteList.AddFavorite(catalogFavorite2);
            await _projectCatalogFavoriteListRepository.Add(favoriteList);

            var handler = new RemoveFromProjectFavoriteListCommandHandler(_projectCatalogFavoriteListRepository, _unitOfWork, _executionContext);
            var cmd = RemoveProjectFavoriteCommand.Create(project.ProjectId.Value,
                new List<Guid> { Any.CatalogFavoriteId().Value, Any.CatalogFavoriteId().Value });
            await Assert.ThrowsAsync<ProjectCatalogFavoriteNotFoundException>(async () => await handler.Handle(cmd, CancellationToken.None));
        }
    }
}
