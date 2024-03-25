using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetProjectFolderSummation;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetProjectFolderSummation
{
    public class GetProjectFolderSummationQueryAuthorizerTest
    {
        [Fact]
        public async Task GetProjectFolderRootAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectFolderSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectFolderSummationQuery.Create(project.ProjectId, Any.ProjectFolderId());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetProjectFolderRootAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectFolderSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectFolderSummationQuery.Create(project.ProjectId, Any.ProjectFolderId());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectFolderRootAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetProjectFolderSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectFolderSummationQuery.Create(project.ProjectId, Any.ProjectFolderId());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetProjectFolderRootAsNonOwnerOrParticipantOrProjectManager_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetProjectFolderSummationQueryAuthorizer(projectRepository, executionContext);
            var query = GetProjectFolderSummationQuery.Create(project.ProjectId, Any.ProjectFolderId());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
