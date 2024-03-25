using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectParticipants;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectParticipants
{
    public class GetProjectParticipantsQueryAuthorizerTests
    {
        private readonly ProjectInMemoryRepository _projectInMemoryRepository;
        private readonly FakeExecutionContext _executionContext;

        public GetProjectParticipantsQueryAuthorizerTests()
        {
            _projectInMemoryRepository = new ProjectInMemoryRepository();
            _executionContext = new FakeExecutionContext();
        }

        [Fact]
        public async Task GetProjectParticipantsAsOwner_ShouldBeAuthorized()
        {
            var project = Any.Project().OwnedBy(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectParticipantsQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectParticipantsQuery.Create(project.ProjectId),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectParticipantsAsParticipant_ShouldBeAuthorized()
        {
            var project = Any.Project().WithParticipant(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectParticipantsQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectParticipantsQuery.Create(project.ProjectId),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectParticipantsAsProjectManager_ShouldBeAuthorized()
        {
            var project = Any.Project().WithProjectManager(_executionContext.UserId);
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectParticipantsQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectParticipantsQuery.Create(project.ProjectId),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectParticipantsAsNonOwnerOrParticipantOrProjectManager_ShouldNotBeAuthorized()
        {
            var project = Any.Project();
            await _projectInMemoryRepository.Add(project);

            var authorizer = new GetProjectParticipantsQueryAuthorizer(_projectInMemoryRepository, _executionContext);

            var authorizationResult =
                await authorizer.Authorize(GetProjectParticipantsQuery.Create(project.ProjectId),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}