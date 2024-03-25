using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.UpdateFolderSupplements;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.UpdateFolderSupplements
{
    public class UpdateFolderSupplementsCommandAuthorizerTest
    {
        [Fact]
        public async Task UpdateFolderSupplementsAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new UpdateFolderSupplementsCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateFolderSupplementsCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Any.FolderSupplements()), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task UpdateFolderSupplementsAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new UpdateFolderSupplementsCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(
                    UpdateFolderSupplementsCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Any.FolderSupplements()), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
