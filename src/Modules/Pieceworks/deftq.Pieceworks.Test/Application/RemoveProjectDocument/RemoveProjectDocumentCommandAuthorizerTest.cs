using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.RemoveDocument;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.RemoveProjectDocument
{
    public class RemoveProjectDocumentCommandAuthorizerTest
    {
        [Fact]
        public async Task RemoveProjectDocumentAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectDocumentCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                RemoveProjectDocumentCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task RemoveProjectDocumentAsNonOwner_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new RemoveProjectDocumentCommandAuthorizer(projectRepository, executionContext);
            var authorizationResult =
                await authorizer.Authorize(RemoveProjectDocumentCommand.Create(project.ProjectId.Value, Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
