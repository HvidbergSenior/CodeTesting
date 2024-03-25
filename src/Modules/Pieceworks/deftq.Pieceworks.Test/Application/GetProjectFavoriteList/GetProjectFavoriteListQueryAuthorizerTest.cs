using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetFavoriteList;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectFavoriteList
{
    public class GetProjectFavoriteListQueryAuthorizerTest
    {
        private readonly ProjectInMemoryRepository _projectInMemoryRepository;
        private readonly FakeExecutionContext _executionContext;

        public GetProjectFavoriteListQueryAuthorizerTest()
        {
            _projectInMemoryRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GetProjectFavoriteListAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectFavoriteListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectFavoriteListQuery.Create(project.ProjectId.Value),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectFavoriteListAsParticipant_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectFavoriteListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectFavoriteListQuery.Create(project.ProjectId.Value),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectFavoriteListAsProjectManager_ShouldBeAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectFavoriteListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectFavoriteListQuery.Create(project.ProjectId.Value),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectFavoriteListAsNonOwnerOrParticipantOrProjectManager_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectFavoriteListQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectFavoriteListQuery.Create(project.ProjectId.Value),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
