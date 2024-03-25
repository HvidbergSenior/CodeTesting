using deftq.BuildingBlocks.Fakes;
using deftq.Pieceworks.Application.GetLogBookAsSpreadSheet;
using deftq.Pieceworks.Infrastructure;
using Xunit;

namespace deftq.Pieceworks.Test.Application.GetLogBookAsSpreadSheet
{
    public class GetLogBookAsSpreadSheetQueryAuthorizerTest
    {
        [Fact]
        public async Task GetLogBookAsSpreadSheetAsOwner_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().OwnedBy(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetLogBookAsSpreadSheetQueryHandler.GetLogBookAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetLogBookAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetLogBookAsSpreadSheetAsParticipants_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithParticipant(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetLogBookAsSpreadSheetQueryHandler.GetLogBookAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetLogBookAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }

        [Fact]
        public async Task GetLogBookAsSpreadSheetAsProjectManager_ShouldBeAuthorized()
        {
            var projectRepository = new ProjectInMemoryRepository();
            var executionContext = new FakeExecutionContext();
            var project = Any.Project().WithProjectManager(executionContext.UserId);
            await projectRepository.Add(project);

            var authorizer =
                new GetLogBookAsSpreadSheetQueryHandler.GetLogBookAsSpreadSheetQueryAuthorizer(projectRepository, executionContext);

            var authorizationResult = await authorizer.Authorize(
                GetLogBookAsSpreadSheetQuery.Create(project.ProjectId.Value),
                CancellationToken.None);

            Assert.True(authorizationResult.IsAuthorized);
        }
    }
}
