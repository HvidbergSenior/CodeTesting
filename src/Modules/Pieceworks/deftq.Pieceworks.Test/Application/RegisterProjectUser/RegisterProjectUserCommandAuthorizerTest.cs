using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RegisterProjectUser;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RegisterProjectUser
{
    public class RegisterProjectUserCommandAuthorizerTest
    {
        [Fact]
        public async Task RegisterProjectUserAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectUserCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterProjectUserCommand.Create(project.ProjectId.Value, Any.ProjectParticipant()),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterProjectUserAsManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectUserCommandAuthorizer(projectRepository, executionContext);

            var authorizationResult =
                await authorizer.Authorize(RegisterProjectUserCommand.Create(project.ProjectId.Value, Any.ProjectParticipant()),
                    CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RegisterProjectUserAsNonOwnerOrManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RegisterProjectUserCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RegisterProjectUserCommand.Create(project.ProjectId.Value, Any.ProjectParticipant()),
                    CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
