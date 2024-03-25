using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectLogBookWeek
{
    public class GetProjectLogBookWeekQueryAuthorizerTest
    {
        [Fact]
        public async Task GetLogBookWeekAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookWeekQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectLogBookWeekQuery.Create(project.ProjectId, executionContext.UserId, 2022, 35),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetLogBookWeekAsNonParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookWeekQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectLogBookWeekQuery.Create(project.ProjectId, new Guid(), 2022, 35),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetLogBookWeekForOtherParticipantAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var otherParticipant = Any.Guid();
            var project = Any.Project().WithParticipant(executionContext.UserId).WithParticipant(otherParticipant);
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookWeekQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectLogBookWeekQuery.Create(project.ProjectId, otherParticipant, 2022, 35),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetLogBookWeekForOwnerAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var owner = Any.Guid();
            var project = Any.Project().OwnedBy(owner).WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookWeekQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectLogBookWeekQuery.Create(project.ProjectId, owner, 2022, 35),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
