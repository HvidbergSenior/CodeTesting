using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetWorkItems;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetWorkItems
{
    public class GetWorkItemsQueryAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectRepository;
        private readonly FakeExecutionContext _executionContext;

        public GetWorkItemsQueryAuthorizerTest()
        {
            _projectRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GetWorkItemsAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectRepository.Add(project);

            var authorizer = new GetWorkItemsQueryAuthorizer(_projectRepository, _executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetWorkItemsQuery.Create(project.ProjectId.Value, Any.ProjectFolderId().Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetWorkItemsAsNonOwnerOrParticipant_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectRepository.Add(project);

            var authorizer = new GetWorkItemsQueryAuthorizer(_projectRepository, _executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetWorkItemsQuery.Create(project.ProjectId.Value, Any.ProjectFolderId().Value),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
