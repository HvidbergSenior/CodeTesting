using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectLumpSum;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectLumpSum
{
    public class UpdateProjectLumpSumCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectLumpSumsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectLumpSumCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectLumpSumCommand.Create(project.ProjectId.Value, 667.95m), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectLumpSumAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer = new UpdateProjectLumpSumCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectLumpSumCommand.Create(project.ProjectId.Value, 667.95m), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
