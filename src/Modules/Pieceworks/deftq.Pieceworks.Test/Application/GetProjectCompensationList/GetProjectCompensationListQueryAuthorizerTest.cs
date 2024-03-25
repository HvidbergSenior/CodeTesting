using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectCompensation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectCompensationList
{
    public class GetProjectCompensationListQueryAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectInMemoryRepository;
        private readonly FakeExecutionContext _executionContext;

        public GetProjectCompensationListQueryAuthorizerTest()
        {
            _projectInMemoryRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GetProjectCompensationListAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectCompensationListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult = await authorizer.Authorize(GetProjectCompensationListQuery.Create(project.ProjectId.Value), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectCompensationListAsNonOwner_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectCompensationListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectCompensationListQuery.Create(project.ProjectId.Value), CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
