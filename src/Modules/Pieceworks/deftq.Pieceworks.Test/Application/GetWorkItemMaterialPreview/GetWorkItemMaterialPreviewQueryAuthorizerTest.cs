using deftq.BuildingBlocks.Fakes;
using deftq.BuildingBlocks.Time;
using deftq.Pieceworks.Application.GetWorkItemMaterialPreview;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetWorkItemMaterialPreview
{
    public class GetWorkItemMaterialPreviewQueryAuthorizerTest
    {
        [Fact]
        public async Task PreviewAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();

            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer = new GetWorkItemMaterialPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemMaterialPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<SupplementOperation>(), new List<Supplement>());
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

            var authorizer = new GetWorkItemMaterialPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemMaterialPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<SupplementOperation>(), new List<Supplement>());
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

            var authorizer = new GetWorkItemMaterialPreviewQueryAuthorizer(projectRepository, executionContext);

            var query = GetWorkItemMaterialPreviewQuery.Create(project.Id, Guid.NewGuid(), new SystemTime().Today(), 0, 0,
                new List<SupplementOperation>(), new List<Supplement>());
            var authorizationResult = await authorizer.Authorize(query, CancellationToken.None);

            Assert.False(authorizationResult.IsAuthorized);
        }
    }
}
