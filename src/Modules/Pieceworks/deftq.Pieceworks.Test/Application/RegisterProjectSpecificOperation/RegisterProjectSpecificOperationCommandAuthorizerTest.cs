using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterExtraWorkAgreement;
using deftq.Pieceworks.Application.RegisterProjectSpecificOperation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectSpecificOperation
{
    public class RegisterProjectSpecificOperationCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterProjectSpecificOperationAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterProjectSpecificOperationAsProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterProjectSpecificOperationAsParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectSpecificOperationCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    RegisterProjectSpecificOperationCommand.Create(project.ProjectId.Value, Any.Guid(), Any.String(10), Any.String(10),
                        Any.String(10), 100000, 100000), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
