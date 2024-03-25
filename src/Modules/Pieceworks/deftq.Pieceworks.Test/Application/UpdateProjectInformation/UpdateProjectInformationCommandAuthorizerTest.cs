using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectInformation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectName
{
    public class UpdateProjectInformationCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateNameAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectInformationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectInformationCommand.Create(project.ProjectId.Value, "test name", "test description", "934820", "4520398"),
                    CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateNameAsNonOwner_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectInformationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectInformationCommand.Create(project.ProjectId.Value, "test name", "test description", "934820", "4520398"),
                CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
