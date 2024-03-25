using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectSummation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectSummation
{
    public class GetProjectSummationQueryAuthorizerTest
    {
        [Fact]
        public async Task GetProjectSummationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectSummationAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectSummationAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectSummationAsNonOwnerOrParticipantOrProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectSummationQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
