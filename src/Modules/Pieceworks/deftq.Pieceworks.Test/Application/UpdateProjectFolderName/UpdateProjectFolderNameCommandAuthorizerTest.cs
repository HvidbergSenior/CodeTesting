using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderName;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateProjectFolderName
{
    public class UpdateProjectFolderNameCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateProjectFolderNameAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderNameCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectFolderNameCommand.Create(project.ProjectId.Value, Guid.NewGuid(), "test name"), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateProjectFolderNameAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer = new UpdateProjectFolderNameCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateProjectFolderNameCommand.Create(project.ProjectId.Value, Guid.NewGuid(), "test name"), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
