using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveProject;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProject
{
    public class RemoveProjectCommandAuthorizerTest
    {
        [Fact]
        public async Task RemoveProjectAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveProjectCommand.Create(project.ProjectId.Value), CancellationToken.None);
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task RemoveProjectAsNonOwner_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(RemoveProjectCommand.Create(project.ProjectId.Value), CancellationToken.None);
            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
