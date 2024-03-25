using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProjectSpecificOperations;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectSpecificOperations
{
    public class RemoveProjectSpecificOperationsCommandAuthorizerTest
    {
        [Fact]
        public async Task RemoveProjectSpecificOperationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectSpecificOperationsCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveProjectSpecificOperationsCommand.Create(project.ProjectId.Value, new List<Guid>()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveProjectSpecificOperationAsProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectSpecificOperationsCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveProjectSpecificOperationsCommand.Create(project.ProjectId.Value, new List<Guid>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveProjectSpecificOperationAsParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectSpecificOperationsCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RemoveProjectSpecificOperationsCommand.Create(project.ProjectId.Value, new List<Guid>()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
