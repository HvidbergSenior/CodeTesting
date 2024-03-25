using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProject;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProject
{
    public class GetProjectQueryAuthorizerTest
    {
        [Fact]
        public async Task GetProjectAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetProjectQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectAsNonOwnerOrParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetProjectQuery.Create(project.ProjectId), CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
