using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateProjectFolderLock;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.LockProjectFolder
{
    public class LockProjectFolderCommandAuthorizerTest
    {
        [Fact]
        public async Task LockFolderAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderLockCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectFolderLockCommand.Create(project.ProjectId.Value, Guid.NewGuid(), UpdateProjectFolderLockCommand.Lock.Locked, false),
                CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task LockFolderAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateProjectFolderLockCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(
                UpdateProjectFolderLockCommand.Create(project.ProjectId.Value, Guid.NewGuid(), UpdateProjectFolderLockCommand.Lock.Locked, false),
                CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
