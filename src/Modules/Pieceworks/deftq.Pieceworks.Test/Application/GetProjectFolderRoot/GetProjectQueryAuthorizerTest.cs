using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectFolderRoot;
using deftq.Pieceworks.Domain.project;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectFolderRoot
{
    public class GetProjectFolderRootQueryAuthorizerTest
    {
        [Fact]
        public async Task GetProjectFolderRootAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);
            
            var authorizer =
                new GetProjectFolderRootQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectFolderRootQuery.Create(project.ProjectId),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectFolderRootAsNonOwnerOrParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);
            
            var authorizer =
                new GetProjectFolderRootQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetProjectFolderRootQuery.Create(project.ProjectId),
                CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
