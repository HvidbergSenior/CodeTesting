using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CopyProjectFolder;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CopyProjectFolder
{
    public class CopyProjectFolderCommandAuthorizerTest
    {
        [Fact]
        public async Task CopyProjectFolderAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new CopyProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(CopyProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()),
                    CancellationToken.None);
            
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task CopyProjectFolderAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new CopyProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult =
                await authorizer.Authorize(CopyProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()),
                    CancellationToken.None);
            
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
