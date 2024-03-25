using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectUsers;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectUsers
{
    public class GetProjectUsersQueryAuthorizerTest
    {
        [Fact]
        public async Task GetProjectUsersAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectUsersQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectUsersAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectUsersQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectUsersAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectUsersQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectUsersAsNonOwnerOrParticipantOrProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectUsersQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectUsersQuery.Create(project.ProjectId);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
