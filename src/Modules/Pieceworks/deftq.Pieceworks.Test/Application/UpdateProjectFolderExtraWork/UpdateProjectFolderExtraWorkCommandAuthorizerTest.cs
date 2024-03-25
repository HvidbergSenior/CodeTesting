using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderExtraWork;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectFolderExtraWork
{
    public class UpdateProjectFolderNameCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectFolderExtraWorkAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderExtraWorkCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectFolderExtraWorkCommand.Create(project.ProjectId.Value, Guid.NewGuid(),
                    UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectFolderExtraWorkAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderExtraWorkCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectFolderExtraWorkCommand.Create(project.ProjectId.Value, Guid.NewGuid(),
                    UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectFolderNameAsNonOwnerAndNonProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderExtraWorkCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectFolderExtraWorkCommand.Create(project.ProjectId.Value, Guid.NewGuid(),
                    UpdateProjectFolderExtraWorkCommand.ExtraWork.ExtraWork), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
