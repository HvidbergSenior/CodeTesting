using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectLogBookWeek;
using deftq.Pieceworks.Domain.projectLogBook;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectLogBookWeek
{
    public class RegisterLogBookWeekCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterLogBookWeekAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectLogBookWeekCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                RegisterProjectLogBookWeekCommand.Create(project.ProjectId.Value, executionContext.UserId, 2022, 32, "", new List<ProjectLogBookDay>()),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RegisterLogBookWeekForParticipant_AsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var participant = Any.ProjectParticipant(executionContext.UserId);
            var project = Any.Project().OwnedBy(executionContext.UserId).WithParticipant(participant.Id);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectLogBookWeekCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                RegisterProjectLogBookWeekCommand.Create(project.ProjectId.Value, participant.Id, 2022, 32, "", new List<ProjectLogBookDay>()),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterLogBookWeekAsParticipant_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var participant = Any.ProjectParticipant(executionContext.UserId);
            var project = Any.Project().WithParticipant(participant.Id);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectLogBookWeekCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                RegisterProjectLogBookWeekCommand.Create(project.ProjectId.Value, participant.Id, 2022, 32, "", new List<ProjectLogBookDay>()),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RegisterLogBookWeekAsNonOwner_AndNonParticipant_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectLogBookWeekCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                RegisterProjectLogBookWeekCommand.Create(project.ProjectId.Value, Guid.NewGuid(), 2022, 32, "", new List<ProjectLogBookDay>()),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
