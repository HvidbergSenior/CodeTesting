using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveFromProjectFavorite;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectFavorite
{
    public class RemoveProjectFavoriteCommandAuthorizerTest
    {
        [Fact]
        public async Task RemoveProjectFavoritesAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectFavoriteCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RemoveProjectFavoriteCommand.Create(project.ProjectId.Value, new List<Guid>()),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveProjectFavoritesAsProjectManager_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectFavoriteCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RemoveProjectFavoriteCommand.Create(project.ProjectId.Value, new List<Guid>()),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveProjectFavoritesAsNonOwnerAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectFavoriteCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RemoveProjectFavoriteCommand.Create(project.ProjectId.Value, new List<Guid>()),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
