using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CloseProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CloseProjectLogBookWeek
{
    public class CloseProjectLogBookWeekCommandAuthorizerTest
    {
        [Fact]
        public async Task CloseLogBookWeekAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new CloseProjectLogBookWeekCommandHandler.CloseProjectLogBookWeekCommandAuthorizer(projectRepository,
                    uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                CloseProjectLogBookWeekCommand.Create(project.ProjectId.Value, executionContext.UserId, 2022, 32),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task CloseLogBookWeekAsNonParticipant_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer =
                new CloseProjectLogBookWeekCommandHandler.CloseProjectLogBookWeekCommandAuthorizer(projectRepository,
                    uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                CloseProjectLogBookWeekCommand.Create(project.ProjectId.Value, new Guid(), 2022, 32),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task CloseOwnLogBookWeek_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new CloseProjectLogBookWeekCommandHandler.CloseProjectLogBookWeekCommandAuthorizer(projectRepository,
                    uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                CloseProjectLogBookWeekCommand.Create(project.ProjectId.Value, executionContext.UserId, 2022, 32),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
