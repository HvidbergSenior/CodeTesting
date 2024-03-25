using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectLogBook;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectLogBook
{
    public class GetDocumentQueryAuthorizerTest
    {
        [Fact]
        public async Task GetLogBookAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(GetProjectLogBookQuery.Create(project.ProjectId), CancellationToken.None);
            
            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetDocumentAsNonOwner_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectLogBookQueryAuthorizer(projectRepository, executionContext);
            var authorizationResult = await authorizer.Authorize(
                GetProjectLogBookQuery.Create(project.ProjectId),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized); 
        }
    }
}
