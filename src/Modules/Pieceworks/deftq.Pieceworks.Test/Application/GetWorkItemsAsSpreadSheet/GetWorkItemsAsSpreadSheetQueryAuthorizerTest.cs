using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetWorkItemsAsSpreadSheet;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetWorkItemsAsSpreadSheet
{
    public class GetWorkItemsAsSpreadSheetQueryAuthorizerTest
    {
        [Fact]
        public async Task GetWorkItemsAsSpreadSheetAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetWorkItemsAsSpreadSheetQueryHandler.GetWorkItemsAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetWorkItemsAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetWorkItemsAsSpreadSheetAsParticipants_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetWorkItemsAsSpreadSheetQueryHandler.GetWorkItemsAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetWorkItemsAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
        
        [Fact]
        public async Task GetWorkItemsAsSpreadSheetAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetWorkItemsAsSpreadSheetQueryHandler.GetWorkItemsAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetWorkItemsAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
