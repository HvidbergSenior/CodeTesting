using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.CreateProjectFolder;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.CreateProjectFolder
{
    public class CreateProjectFolderCommandAuthorizerTest
    {
        [Fact]
        public async Task CreateProjectFolderAsOwner_ShouldBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var authorizer = new CreateProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(CreateProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(),  "folder", "description"),
                CancellationToken.None);
            
            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task CreateProjectFolderAsNonOwner_ShouldNotBeAuthorized()
        {
            var uow = new FakeUnitOfWork();
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer = new CreateProjectFolderCommandAuthorizer(projectRepository, uow, executionContext);
            var authorizationResult = await authorizer.Authorize(CreateProjectFolderCommand.Create(project.ProjectId.Value, Guid.NewGuid(),  "folder", "description"),
                CancellationToken.None);
            
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
