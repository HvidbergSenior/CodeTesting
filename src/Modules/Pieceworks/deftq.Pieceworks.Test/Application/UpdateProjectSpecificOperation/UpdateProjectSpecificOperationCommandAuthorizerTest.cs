using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectSpecificOperation
{
    public class UpdateProjectSpecificOperationCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectSpecificOperationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectSpecificOperationAsProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectSpecificOperationAsParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
