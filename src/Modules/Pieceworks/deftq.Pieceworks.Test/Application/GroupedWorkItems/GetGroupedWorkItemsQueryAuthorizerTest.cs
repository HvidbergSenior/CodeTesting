using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetGroupedWorkItems;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GroupedWorkItems
{
    public class GetGroupedWorkItemsQueryAuthorizerTest
    {
        [Fact]
        public async Task GetGroupedWorkItemsAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetGroupedWorkItemsQueryAuthorizer(projectRepository, executionContext);

            var query = GetGroupedWorkItemsQuery.Create(project.Id, Guid.NewGuid(), 10);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetGroupedWorkItemsAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetGroupedWorkItemsQueryAuthorizer(projectRepository, executionContext);

            var query = GetGroupedWorkItemsQuery.Create(project.Id, Guid.NewGuid(), 10);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetGroupedWorkItemsAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetGroupedWorkItemsQueryAuthorizer(projectRepository, executionContext);

            var query = GetGroupedWorkItemsQuery.Create(project.Id, Guid.NewGuid(), 10);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetGroupedWorkItemsAsNonOwnerAndNonParticipantAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetGroupedWorkItemsQueryAuthorizer(projectRepository, executionContext);

            var query = GetGroupedWorkItemsQuery.Create(project.Id, Guid.NewGuid(), 10);
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
