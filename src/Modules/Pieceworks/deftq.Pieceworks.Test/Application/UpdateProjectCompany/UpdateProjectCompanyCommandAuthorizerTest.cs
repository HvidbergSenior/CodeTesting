using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectCompany;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectCompany
{
    public class UpdateProjectCompanyCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectCompanyAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectCompanyCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateProjectCompanyCommand.Create(project.ProjectId.Value, String.Empty, String.Empty, String.Empty, String.Empty);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectCompanyAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectCompanyCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateProjectCompanyCommand.Create(project.ProjectId.Value, String.Empty, String.Empty, String.Empty, String.Empty);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectCompanyAsNonOwnerOrNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectCompanyCommandAuthorizer(projectRepository, executionContext);
            var command = UpdateProjectCompanyCommand.Create(project.ProjectId.Value, String.Empty, String.Empty, String.Empty, String.Empty);
            var authorizationResult = await authorizer.Authorize(command, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
