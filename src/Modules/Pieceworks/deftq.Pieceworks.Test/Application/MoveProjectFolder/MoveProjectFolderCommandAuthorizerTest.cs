using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.MoveProjectFolder;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.MoveProjectFolder
{
    public class MoveProjectFolderCommandAuthorizerTest
    {
        [Fact]
        public async Task MoveProjectFolderAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var authorizer = new MoveProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(MoveProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()),
                CancellationToken.None);
            
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task MoveProjectFolderAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer = new MoveProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(MoveProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()),
                CancellationToken.None);
            
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
