using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.OpenProjectLogBookWeek;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.OpenProjectLogBookWeek
{
    public class OpenProjectLogBookWeekCommandAuthorizerTest
    {
        [Fact]
        public async Task OpenLogBookWeekAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new OpenProjectLogBookWeekCommandHandler.OpenProjectLogBookWeekCommandAuthorizer(projectRepository,
                    uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                OpenProjectLogBookWeekCommand.Create(project.ProjectId.Value, executionContext.UserId, 2022, 32),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task OpenLogBookWeekAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer =
                new OpenProjectLogBookWeekCommandHandler.OpenProjectLogBookWeekCommandAuthorizer(projectRepository,
                    uow, executionContext);

            var authorizationResult = await authorizer.Authorize(
                OpenProjectLogBookWeekCommand.Create(project.ProjectId.Value, executionContext.UserId, 2022, 32),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
