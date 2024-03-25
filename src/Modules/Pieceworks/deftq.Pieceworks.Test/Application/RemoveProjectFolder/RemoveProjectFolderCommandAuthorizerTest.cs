using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProjectFolder;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectFolder
{
    public class RemoveProjectFolderCommandAuthorizerTest
    {
        [Fact]
        public async Task RemoveProjectFolderAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var authorizer = new RemoveProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid()),
                CancellationToken.None);
            
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RemoveProjectFolderAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer = new RemoveProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid()),
                CancellationToken.None);
            
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
