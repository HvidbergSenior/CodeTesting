using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectCatalogFavorite;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Domain.ProjectCatalogFavorite;
using deftq.Pieceworks.Infrastructure;
using FluentAssertions;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectCatalogFavorite
{
    public class RegisterProjectCatalogFavoriteCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterCatalogFavoriteAsOwner_ShouldBeAuthorized()
        {
            // Given project with owner
            var ctx = CancellationToken.None;
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project, ctx);

            // When authorizing
            var authorizer = new RegisterProjectCatalogFavoriteCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectCatalogFavoriteCommand.Create(project.ProjectId.Value, Any.Guid(), CatalogItemType.Material, Any.Guid(),
                        Any.String(10), Any.String(10), Any.String(10)), ctx);

            // Then owner is authorized
            authorizationResult.IsAuthorized.Should().BeTrue();
        }
        
        [Fact]
        public async Task RegisterCatalogFavoriteAsProjectManager_ShouldBeAuthorized()
        {
            // Given project with project manager
            var ctx = CancellationToken.None;
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project, ctx);

            // When authorizing
            var authorizer = new RegisterProjectCatalogFavoriteCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectCatalogFavoriteCommand.Create(project.ProjectId.Value, Any.Guid(), CatalogItemType.Material, Any.Guid(),
                        Any.String(10), Any.String(10), Any.String(10)), ctx);

            // Then project manager is authorized
            authorizationResult.IsAuthorized.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterCatalogFavoriteAsParticipant_ShouldNotBeAuthorized()
        {
            // Given project with participant
            var ctx = CancellationToken.None;
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            project.AddProjectParticipant(Any.ProjectParticipant(executionContext.UserId, ProjectParticipantName.Create(executionContext.UserName)));
            await projectRepository.Add(project, ctx);

            // When authorizing
            var authorizer = new RegisterProjectCatalogFavoriteCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterProjectCatalogFavoriteCommand.Create(project.ProjectId.Value, Any.Guid(), CatalogItemType.Material,
                    Any.Guid(), Any.String(10), Any.String(10), Any.String(10)), ctx);

            // Then participant is not authorized
            authorizationResult.IsAuthorized.Should().BeFalse();
        }
    }
}
