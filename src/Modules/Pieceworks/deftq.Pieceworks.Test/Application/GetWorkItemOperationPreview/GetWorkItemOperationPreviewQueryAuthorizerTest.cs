using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetWorkItemOperationPreview;
using deftq.Pieceworks.Infrastructure;
using Xunit;
using Supplement = deftq.Pieceworks.Application.GetWorkItemOperationPreview.Supplement;

namespace deftq.Pieceworks.Test.Application.GetWorkItemOperationPreview
{
    public class GetWorkItemOperationPreviewQueryAuthorizerTest
    {
        [Fact]
        public async Task PreviewAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetWorkItemOperationPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemOperationPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<Supplement>());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task PreviewAsParticipant_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetWorkItemOperationPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemOperationPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<Supplement>());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task PreviewAsNonOwnerOrParticipant_ShouldNotBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project();
            await projectRepository.Add(project);

            var authorizer = new GetWorkItemOperationPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemOperationPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<Supplement>());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
